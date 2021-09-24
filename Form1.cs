using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Lab5
{
    public partial class Form1 : Form
    {

        ModuleFactory moduleFactory = new ModuleFactory();

        public Form1()
        {
            InitializeComponent();
        }

        private string GetModules()
        {
            string allLines = "";
            try
            {
                string originalPath = AppDomain.CurrentDomain.BaseDirectory;
                string path = Path.GetFullPath(System.IO.Path.Combine(originalPath, @"..\..\..\resources\" + "modules.txt"));
                //MessageBox.Show(path);

                StreamReader reader = File.OpenText(path);
                allLines = reader.ReadToEnd();

                reader.Close();
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine(err.Message);
            }

            return allLines;
        }

        public void SetLabelValue(int value)
        {
            valueLabel.Text = value.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int counter = 1;
            foreach (string item in this.GetModules().Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None))
            {
                Button button = new Button();
                button.Text = item;
                button.Location = new Point(100 * counter, 40);
                button.ForeColor = Color.Red;
                button.Click += new System.EventHandler(MenuItem_Click);
                panel1.Controls.Add(button);
                counter++;

            }
        }

        private void MenuItem_Click(object sender, EventArgs e)
        {

            string moduleName = ((Button)sender).Text;
            IModule module = moduleFactory.CreateModule(moduleName);

            if(module != null)
            {
                SetLabelValue(module.Compute());
            }
            else
            {
                MessageBox.Show("The selected operation is not implemented in the program, please choose another operation.");
            }

        }

    }

    public interface IModuleFactory
    {
        public IModule CreateModule(string module);
    }

    public class ModuleFactory : IModuleFactory
    {
        public IModule CreateModule(string module)
        {
            string officalName = "Lab5." + module + "Module";
            System.Diagnostics.Debug.WriteLine(officalName);

            try
            {
                Type t = Type.GetType(officalName);
                Object o = Activator.CreateInstance(t);
                return (IModule)o;
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine(err.Message);
            }
            return null;
        }
    }



    public interface IModule
    {
        public int Compute();
    }

    public abstract class AbstractModule : IModule
    {
        private static int _currentValue = 0;

        public abstract int Compute();

        public static int GetCurrentValue()
        {
            return _currentValue;
        }

        public static void SetCurrentValue(int num)
        {
            _currentValue = num;

        }
    }

    public interface IModuleWithInput
    {
        public int LocalCompute();

        public int GetInput();
    }

    public interface IModuleWithOutInput
    {
        public int LocalCompute();
    }

    public class InitializeModule : AbstractModule, IModuleWithInput
    {
        public override int Compute()
        {
            int newValue = LocalCompute();

            SetCurrentValue(newValue);

            return newValue;
        }

        public int GetInput()
        {
            return Prompt.ShowDialog("Enter a number", "Enter a number to initiate the Value");
        }

        public int LocalCompute()
        {

            return GetInput();
        }
    }

    public class SumModule : AbstractModule, IModuleWithInput
    {
        public override int Compute()
        {
            int newValue = LocalCompute();

            SetCurrentValue(newValue);

            return newValue;
        }

        public int GetInput()
        {
            return Prompt.ShowDialog("Enter a number", "Enter a number to add to the Value");
        }

        public int LocalCompute()
        {

            return GetInput() + GetCurrentValue();
        }
    }

    public class SubtractModule : AbstractModule, IModuleWithInput
    {

        public override int Compute()
        {
            int newValue = LocalCompute();

            SetCurrentValue(newValue);

            return newValue;
        }

        public int GetInput()
        {
            return Prompt.ShowDialog("Enter a number", "Enter a number to subtract from the Value");
        }

        public int LocalCompute()
        {
            return GetCurrentValue() - GetInput();
        }
    }

    public class ProductModule : AbstractModule, IModuleWithInput
    {
        public override int Compute()
        {
            int newValue = LocalCompute();

            SetCurrentValue(newValue);

            return newValue;
        }

        public int GetInput()
        {
            return Prompt.ShowDialog("Enter a number", "Enter a number to multiply the Value by");
        }

        public int LocalCompute()
        {
            return GetCurrentValue() * GetInput();
        }
    }

    public class PowerModule : AbstractModule, IModuleWithInput
    {
        public override int Compute()
        {
            int newValue = LocalCompute();

            SetCurrentValue(newValue);

            return newValue;
        }

        public int GetInput()
        {
            return Prompt.ShowDialog("Enter a number", "Enter a number to raise the Value power of");
        }

        public int LocalCompute()
        {
            int power = GetInput();
            int currentValue = GetCurrentValue();
            int newValue = currentValue;

            for (int i = 0; i < power; i++)
            {
                newValue *= currentValue;
            }

            return newValue;
        }
    }

    public class LogModule : AbstractModule, IModuleWithOutInput
    {
        public override int Compute()
        {
            int newValue = LocalCompute();

            SetCurrentValue(newValue);

            return newValue;
        }

        public int LocalCompute()
        {
            int counter = 0;
            double e = 2.718;
            double compareNumber = 2.718;
            int currentvalue = GetCurrentValue();

            while (compareNumber < currentvalue)
            {
                counter++;
                compareNumber *= e;
            }

            return counter;
        }
    }

    public static class Prompt
    {
        public static int ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 400,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };

            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 50 };
            Button confirmation = new Button() { Text = "Ok", Left = 120, Width = 100, Top = 50, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;


            return prompt.ShowDialog() == DialogResult.OK ? Int32.Parse(textBox.Text) : 0;
        }
    }
}
