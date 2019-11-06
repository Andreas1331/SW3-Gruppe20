using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SW3Projekt.Views
{
    public partial class ExportView : UserControl
    {
        //CONSTRUCTOR
        public ExportView()
        {
            const int WeeksPerYear = 53; //Weeks that can be selected
            const int StartYear = 2019; //Min. possible year with data (Program creation year)

            InitializeComponent();

            //Generate Weeks that can be selected to comboBoxes
            for (int i = 0; i <= WeeksPerYear; i++)
            {
                FromWeek.Items.Add(i);
                ToWeek.Items.Add(i);
            }

            //Generate Years to comboBoxes
            for (int i = StartYear; i <= DateTime.Now.Year; i++) //Possiple years. From StartYear to current year
            {
                Console.WriteLine(i);
                FromYear.Items.Add(i);
                ToYear.Items.Add(i);
            }

            //Pre-selected text with instuctions
            FromWeek.Text = "Vælg start uge";
            FromYear.Text = "Vælg året for start ugen";

            ToWeek.Text = "Vælg slut uge";
            ToYear.Text = "Vælg året for slut ugen";



            //Right hand side instruction/information for export
            Instructions.Text = "Is he staying arrival address earnest. " +
                "To preference considered it themselves inquietude collecting estimating. " +
                "View park for why gay knew face. Next than near to four so hand. Times so do he downs me would. " +
                "Witty abode party her found quiet law. They door four bed fail now have. Is post each that just leaf no." +
                "He connection interested so we an sympathize advantages. To said is it shed want do.Occasional middletons everything so to." +
                "Have spot part for his quit may.Enable it is square my an regard.Often merit stuff first oh up hills as he.Servants contempt as although addition dashwood is procured." +
                "Interest in yourself an do of numerous feelings cheerful confined. Feet evil to hold long he open knew an no. Apartments occasional boisterous as solicitude to introduced. " +
                "Or fifteen covered we enjoyed demesne is in prepare.In stimulated my everything it literature. Greatly explain attempt perhaps in feeling he." +
                "House men taste bed not drawn joy.Through enquire however do equally herself at.Greatly way old may you present improve. " +
                "Wishing the feeling village him musical.";
        }
    }
}