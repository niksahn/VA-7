using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VA_7 {
    public delegate void fun(double a);
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        public delegate double IntegrationMethod(double a, double b, int n);

        public static class RungeRule {
            public static double ErrorMediumRect(double a, double b, int n) {
                double I_n = MediumRectMethod(a, b, n);
                double I_2n = MediumRectMethod(a, b, 2 * n);
                return Math.Abs(I_2n - I_n) / 3;
            }

            public static double ErrorLeftRect(double a, double b, int n) {
                double I_n = LeftRectMethod(a, b, n);
                double I_2n = LeftRectMethod(a, b, 2 * n);
                return Math.Abs(I_2n - I_n);
            }

            public static double ErrorRightRect(double a, double b, int n) {
                double I_n = RightRectMethod(a, b, n);
                double I_2n = RightRectMethod(a, b, 2 * n);
                return Math.Abs(I_2n - I_n);
            }

            public static double ErrorTrapezoid(double a, double b, int n) {
                double I_n = TrapezoidMethod(a, b, n);
                double I_2n = TrapezoidMethod(a, b, 2 * n);
                return Math.Abs(I_2n - I_n) / 3;
            }

            public static double ErrorSimpsons(double a, double b, int n) {
                double I_n = SimpsonsMethod(a, b, n);
                double I_2n = SimpsonsMethod(a, b, 2 * n);
                return Math.Abs(I_2n - I_n) / 15;
            }
        }

        public static double Integrand(double x) {
            return (1+Math.Sin(2*x))*Math.Pow(Math.E,-x);
        }

        public static double MediumRectMethod(double a, double b, int n) {
            double h = (b - a) / n;
            double sum = 0;

            for (int i = 0; i < n; i++) {
                double xi = a + (i + 0.5) * h;
                sum += Integrand(xi);
            }

            return h * sum;
        }

        public static double LeftRectMethod(double a, double b, int n) {
            double h = (b - a) / n;
            double sum = 0;

            for (int i = 0; i < n; i++) {
                double xi = a + i * h;
                sum += Integrand(xi);
            }

            return h * sum;
        }

        public static double RightRectMethod(double a, double b, int n) {
            double h = (b - a) / n;
            double sum = 0;

            for (int i = 1; i <= n; i++) {
                double xi = a + i * h;
                sum += Integrand(xi);
            }

            return h * sum;
        }

        public static double TrapezoidMethod(double a, double b, int n) {
            double h = (b - a) / n;
            double sum = 0.5 * (Integrand(a) + Integrand(b));

            for (int i = 1; i < n; i++) {
                double xi = a + i * h;
                sum += Integrand(xi);
            }

            return h * sum;
        }

        public static double SimpsonsMethod(double a, double b, int n) {
            if (n % 2 != 0) {
                throw new ArgumentException("Для метода Симпсона кол-во интервалов должно быть четным числом");
            }

            double h = (b - a) / n;
            double sum = Integrand(a) + Integrand(b);

            for (int i = 1; i < n; i += 2) {
                double xi = a + i * h;
                sum += 4 * Integrand(xi);
            }

            for (int i = 2; i < n - 1; i += 2) {
                double xi = a + i * h;
                sum += 2 * Integrand(xi);
            }

            return h * sum / 3;
        }

        private async void button1_Click(object sender, EventArgs e) {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";

            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            richTextBox1.Text = "";
            richTextBox2.Text = "";
            richTextBox3.Text = "";
            richTextBox4.Text = "";
            richTextBox5.Text = "";
            double a = 1;
            double b =2;
            int n = 2;
            double accuracy = (double)numericUpDown2.Value;

            double midpointResult, midpointError,
                   leftRectangleResult, leftRectangleError,
                   rightRectangleResult, rightRectangleError,
                   trapezoidalResult, trapezoidalError,
                   simpsonsResult, simpsonsError;

            var midpointTask = Task.Run(() => CalculateWithAccuracy(MediumRectMethod, a, b, n, accuracy, RungeRule.ErrorMediumRect, 
           (i) =>
           {
                richTextBox1.Invoke(new Action(() => { richTextBox1.AppendText(i + "\n"); }));
           }
           )
            );
            var leftRectangleTask = Task.Run(() => CalculateWithAccuracy(LeftRectMethod, a, b, n, accuracy, RungeRule.ErrorLeftRect, (i) =>
            {
                richTextBox2.Invoke(new Action(() => { richTextBox2.AppendText( i + "\n"); }));
            }));
            var rightRectangleTask = Task.Run(() => CalculateWithAccuracy(RightRectMethod, a, b, n, accuracy, RungeRule.ErrorRightRect, (i) =>
            {
                richTextBox3.Invoke(new Action(() => { richTextBox3.AppendText(i + "\n"); }));
            }));
            var trapezoidalTask = Task.Run(() => CalculateWithAccuracy(TrapezoidMethod, a, b, n, accuracy, RungeRule.ErrorTrapezoid, (i) => { richTextBox4.Invoke(new Action(() => { richTextBox4.AppendText(i + "\n"); })); }));
            try {
                var simpsonsTask = Task.Run(() => CalculateWithAccuracy(SimpsonsMethod, a, b, n, accuracy, RungeRule.ErrorSimpsons, (i) => { richTextBox5.Invoke(new Action(() => { richTextBox5.AppendText(i+"\n"); })); }));
                await Task.WhenAll(midpointTask, leftRectangleTask, rightRectangleTask, trapezoidalTask, simpsonsTask);

                midpointResult = midpointTask.Result.Item1;
                midpointError = midpointTask.Result.Item2;

                leftRectangleResult = leftRectangleTask.Result.Item1;
                leftRectangleError = leftRectangleTask.Result.Item2;

                rightRectangleResult = rightRectangleTask.Result.Item1;
                rightRectangleError = rightRectangleTask.Result.Item2;

                trapezoidalResult = trapezoidalTask.Result.Item1;
                trapezoidalError = trapezoidalTask.Result.Item2;

                simpsonsResult = simpsonsTask.Result.Item1;
                simpsonsError = simpsonsTask.Result.Item2;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                return;
            }

            textBox1.Text = midpointResult.ToString();
            textBox2.Text = leftRectangleResult.ToString();
            textBox3.Text = rightRectangleResult.ToString();
            textBox4.Text = trapezoidalResult.ToString();
            textBox5.Text = simpsonsResult.ToString();

            textBox6.Text = midpointError.ToString();
            textBox7.Text = leftRectangleError.ToString();
            textBox8.Text = rightRectangleError.ToString();
            textBox9.Text = trapezoidalError.ToString();
            textBox10.Text = simpsonsError.ToString();
        }

        private (double result, double error) CalculateWithAccuracy(Func<double, double, int, double> integrationMethod, double a, double b, int n, double accuracy, Func<double, double, int, double> errorMethod, fun fillTable) {
            double result;
            double error;
            do {
                error = errorMethod(a, b, n);
                n *= 2;
                result = integrationMethod(a, b, n);
                fillTable(result);
            } while (error > accuracy);

            return (result, error);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
