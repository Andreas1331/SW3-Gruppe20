using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using SW3Projekt.ViewModels;

namespace SW3Projekt.Tools
{
    public static class Printer
    {
        //PROPERTIES

        public static List<string> Lines = new List<string>(); //List of rows formatted as string ready to be exported

        //METHODS
        public static int Print(string FileName, string OutputLocation)//Prints Lines to a file
        {
            //Find a new file name if file name is already found. Like myFile(i).csv
            string path = OutputLocation + '\\' + FileName + ".csv";

            for (int i = 1; i <= 100; i++) //Only checks 100 times. Meaning if 100 it doesnt print if there are 100 files with the same name
                if (File.Exists(path)) //Make a new name if file exists
                    path = OutputLocation + '\\'+ FileName + $"({i})" + ".csv";
                else
                    break;

            if (File.Exists(path)) //Final check
                return -1;

            //Now print
            foreach (string line in Lines)
            {
                if (Lines.Count != 1) //Make new line if not last line
                    File.AppendAllText(path, line + "\n"); //Write all lines to the new file
                else
                    File.AppendAllText(path, line); //Write all lines to the new file
                Lines.Remove(line);
            }



            return 0;
        }

        // Method to print PDF
        public static void PrintPdf(SaldoOverviewViewModel saldoOverviews)
        {
            // Create document and section
            Document document = new Document();
            Section section = document.AddSection();
            
            section.PageSetup.LeftMargin = 10;
            section.PageSetup.RightMargin = 10;
            // An A4 page is 595 units.

            // Create the page and setup with settings
            CreatePage(document, section, saldoOverviews);

            // Render and save the document
            RenderAndSavePDF(document);
        }

        private static void CreatePage(Document document, Section section, SaldoOverviewViewModel saldoOverviews)
        {
            // Create and style the table
            Table table = section.AddTable();
            table.Style = "Table";
            table.Borders.Width = 0.2;
            table.Borders.Left.Width = 0.2;
            table.Borders.Right.Width = 0.2;
            table.Rows.LeftIndent = 0;

            // Get the headers from SaldoOverview dynamically
            // List to store all the properties
            List<string> columnHeadersToDisplay = new List<string>();

            // Instance to get/set values
            Models.SaldoOverview So = new Models.SaldoOverview();

            // Iterate through all properties and add them to the list and count rows at the same time
            int amountOfRows = 0;
            PropertyInfo[] properties = typeof(Models.SaldoOverview).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                // We sort out Caliburns built-in properties
                switch (property.Name)
                {
                    case "IsActive":
                    case "IsInitialized":
                    case "IsNotifying":
                    case "Parent":
                    case "DisplayName":
                        break;
                    // If not part of built-in props, add to list
                    default:
                        columnHeadersToDisplay.Add(property.Name);
                        Console.WriteLine(property.Name);
                        amountOfRows++;
                        break;
                }
            }

            // Add columns to the table MUST be done BEFORE adding rows
            Column column = new Column();
            for (int i = 0; i < columnHeadersToDisplay.Count; i++)
            {
                double colSize;
                double pageWidth = 575;

                switch (So.TranslateProperties(columnHeadersToDisplay[i]))
                {
                    case "ID":
                        colSize = 0.05 * pageWidth;
                        break;
                    case "Navn":
                        colSize = 0.27 * pageWidth;
                        break; 
                    case "Afs.":
                        colSize = 0.05 * pageWidth;
                        break;
                    case "FerieFri":
                        colSize = 0.1 * pageWidth;
                        break;
                    case "Ferie":
                        colSize = 0.1 * pageWidth;
                        break;
                    case "Syg":
                        colSize = 0.05 * pageWidth;
                        break;
                    case "Arb. timer":
                        colSize = 0.1 * pageWidth;
                        break;
                    case "Telefon":
                        colSize = 0.12 * pageWidth;
                        break;
                    case "Aftrådt":
                        colSize = 0.08 * pageWidth;
                        break;
                    case "Syg i alt":
                        colSize = 0.08 * pageWidth;
                        break;
                    default:
                        colSize = 0;
                        break;
                }
                // Add the column
                column = table.AddColumn(colSize);
                column.Format.Alignment = ParagraphAlignment.Left;
                column.Format.Font.Size = 7;
            }

            // Add a row to the table that acts as header
            Row row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;

            // Add the column headers to row 0
            string columnHeaderNameTranslated;
            for (int i = 0; i < amountOfRows; i++)
            {
                columnHeaderNameTranslated = So.TranslateProperties(columnHeadersToDisplay[i]);
                row.Cells[i].AddParagraph(columnHeaderNameTranslated);
                row.Cells[i].Format.Font.Bold = true;
                row.Cells[i].Format.Font.Size = 7;
                row.Cells[i].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[i].VerticalAlignment = VerticalAlignment.Center;
            }

            // Fill document with data
            FillDocument(document, section, table, saldoOverviews);
            AddTotalHours(table, saldoOverviews);
        }

        private static void FillDocument(Document document, Section section, Table table, SaldoOverviewViewModel saldoOverviews)
        {
            //Add all data from saldoOverviews list.
            foreach (var item in saldoOverviews.SaldoOverviewCollection)
            {
                Row row = table.AddRow();
                row.Cells[0].AddParagraph(item.EmployeeId.ToString());
                row.Cells[1].AddParagraph(item.EmployeeName);
                row.Cells[2].AddParagraph(item.PaidLeave.ToString());
                row.Cells[3].AddParagraph(item.HolidayFree.ToString());
                row.Cells[4].AddParagraph(item.Holiday.ToString());
                row.Cells[5].AddParagraph(item.Illness.ToString());
                row.Cells[6].AddParagraph(item.WorkHours.ToString());
                row.Cells[7].AddParagraph(item.EmployeePhonenumber);
                row.Cells[8].AddParagraph(getTrueFalseAsYesNoInDK(item.IsEmployeeFired));
                row.Cells[9].AddParagraph(item.PercentIllness.ToString() + "%");
            }
        }

        private static string getTrueFalseAsYesNoInDK(bool value)
        {
            if (value == true)
            {
                return "Ja";
            }
            else
            {
                return "Nej";
            }
        }

        private static void AddTotalHours(Table table, SaldoOverviewViewModel saldoOverviews)
        {
            // Add a empty row to make space
            Row rowEmptySpace = table.AddRow();
            rowEmptySpace.Borders.Visible = false;

            //Add the total amount 
            Row rowTotalValues = table.AddRow();
            rowTotalValues.Format.Font.Bold = true;
            // Set thicker borders on the top row with headers
            table.SetEdge(0, 0, table.Columns.Count, 1, Edge.Box, BorderStyle.Single, 0.75);
            // Set thicker borders on the total values
            table.SetEdge(2, table.Rows.Count - 1, 5, 1, Edge.Box, BorderStyle.Single, 0.75);
            // Set borders on average illness box
            table.SetEdge(9, table.Rows.Count - 1, 1, 1, Edge.Box, BorderStyle.Single, 0.75);

            // Remove borders in empty cells
            rowTotalValues.Cells[0].Borders.Visible = false;
            rowTotalValues.Cells[1].Borders.Visible = false;
            rowTotalValues.Cells[7].Borders.Visible = false;
            rowTotalValues.Cells[8].Borders.Visible = false;
            
            // Add data to specific cells
            rowTotalValues.Cells[2].AddParagraph(saldoOverviews.BoxPaidLeaveTotal.ToString());
            rowTotalValues.Cells[3].AddParagraph(saldoOverviews.BoxHolidayFreeTotal.ToString());
            rowTotalValues.Cells[4].AddParagraph(saldoOverviews.BoxHolidayTotal.ToString());
            rowTotalValues.Cells[5].AddParagraph(saldoOverviews.BoxIllnessTotal.ToString());
            rowTotalValues.Cells[6].AddParagraph(saldoOverviews.BoxWorkhoursTotal.ToString());
            rowTotalValues.Cells[9].AddParagraph(saldoOverviews.BoxAvgIllnessPercantage + "%");

        }


        // Method to save, render and print the document
        private static void RenderAndSavePDF(Document document)
        {
            // Create render as pdf
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(false);

            // Pass document to render AND render the document
            pdfRenderer.Document = document;
            pdfRenderer.RenderDocument();

            // To save, first set the filepath
            string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // Now merge the path and name
            string finalDoc = FilePath + @"\TestPDF.pdf";

            // Finally save the document
            pdfRenderer.PdfDocument.Save(finalDoc);

            // Open the document automatically with built-in software
            // MAY BE DELETED
            Process.Start(finalDoc);
        }

    }
}