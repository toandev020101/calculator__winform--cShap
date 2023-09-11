using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calc_Toan
{
    public partial class frmToan : Form
    {
        private List<String> formulaList = new List<String>();
        private List<String> historyList = new List<String>();
        private List<Color> colorList = new List<Color>();
        private List<Color> colorUsedList = new List<Color>();

        public frmToan()
        {
            InitializeComponent();

            // Chỉnh căn lề của văn bản hiện tại (đang được chọn) sang phải
            rtbFormula.SelectionAlignment = HorizontalAlignment.Right;

            // Đặt lề nội dung cho đoạn văn bản hiện tại
            rtbFormula.SelectionIndent = 20; // Lề trái
            rtbFormula.SelectionRightIndent = 20; // Lề phải

            // Nội dung ban đầu
            rtbFormula.Text = "0";
        }

        private float GetBrightness(Color color) // Độ sáng của màu
        {
            return (float)(0.299 * color.R + 0.587 * color.G + 0.114 * color.B);
        }

        private Color GetRandomColor(Random random) // Lấy màu ngẫu nhiên
        {
            return Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
        }

        private void resetForm() // Làm mới form
        {
            rtbFormula.Text = "0";
            tbResult.Text = "";
            formulaList.Clear();
        }

        private String formatNumber(String str)
        {
            String result = "";
            result = str.Replace(".", ",");
            int resCount = result.Length;

            if (result.Contains(","))
            {
                resCount = result.IndexOf(",");
            }

            for (int i = resCount - 3; i > 0; i -= 3)
            {
                result = result.Insert(i, ".");
            }

            return result;
        }

        private void updateRtbFormula()
        {
            if(formulaList.Count > 0)
            {
                rtbFormula.Text = "";

                // cập nhật chuỗi
                formulaList.ForEach(str => {
                    if (str == "+" || str == "-" || str == "x" || str == "÷") // phép toán
                    {
                        str = " " + str + " ";
                    }
                    else if (double.TryParse(str, out double value)) // cập nhật số vd 9384.293 => 9.384,293
                    {
                        str = formatNumber(str);
                    }

                    rtbFormula.Text += str;
                });

                // đổi màu
                String rtbFormulaText = rtbFormula.Text;
                Random random = new Random();
                int indexColor = 0;
                Color colorMath = Color.FromArgb(45, 122, 187);
                int minBrightness = 150; // độ sáng tối thiểu của màu

                // random màu nếu list màu không đủ
                int breaketCount = formulaList.Count(str => str == "(");
                if(breaketCount > colorList.Count)
                {
                    for(int i = 1; i <= breaketCount - colorList.Count; i++)
                    {
                        Color colorRandom;
                        // loại bỏ màu nền, màu phép toán, độ sáng, màu đã dùng
                        do
                        {
                            colorRandom = GetRandomColor(random);
                            colorList.Add(colorRandom);
                        } while (colorRandom == colorMath || colorRandom == Color.Black 
                        || GetBrightness(colorRandom) < minBrightness || !colorList.Contains(colorRandom));
                    }
                }

                for (int i = 0; i < rtbFormulaText.Length; i++)
                {
                    if (rtbFormulaText[i] == '+' || rtbFormulaText[i] == '-' 
                        || rtbFormulaText[i] == 'x' || rtbFormulaText[i] == '÷')
                    {
                        rtbFormula.Select(i, 1);
                        rtbFormula.SelectionColor = colorMath;
                    }else if (rtbFormulaText[i] == '(')
                    {
                        rtbFormula.Select(i, 1);
                        rtbFormula.SelectionColor = colorList[indexColor];
                        colorUsedList.Add(colorList[indexColor]);
                        indexColor++;
                    }else if (rtbFormulaText[i] == ')')
                    {
                        rtbFormula.Select(i, 1);
                        rtbFormula.SelectionColor = colorUsedList.LastOrDefault();
                        colorUsedList.RemoveAt(colorUsedList.Count - 1);
                    }
                }
            }
            else
            {
                rtbFormula.Text = "0";
            }
        }

        private void addFormulaList(int number)
        {
            if (tbResult.Text.Length > 0)
            {
                resetForm();
            }

            if (double.TryParse(formulaList.LastOrDefault(), out double value)) // cộng tiếp vào chuỗi
            {
                if (value == 0 && !formulaList.LastOrDefault().Contains("."))
                {
                    formulaList[formulaList.Count - 1] = number.ToString();
                }
                else
                {
                    formulaList[formulaList.Count - 1] = formulaList.LastOrDefault() + number;
                }
            }
            else // thêm mới
            {
                formulaList.Add(number.ToString());
            }
            updateRtbFormula();
        }

        private void addFormulaList(String str)
        {
            if (tbResult.Text.Length > 0)
            {
                resetForm();
            }

            if (formulaList.Count > 0)
            {
                if (str == ".")
                {
                    if (double.TryParse(formulaList.LastOrDefault(), out double value)
                    && !formulaList.LastOrDefault().Contains("."))
                    {
                        formulaList[formulaList.Count - 1] = formulaList.LastOrDefault() + str;
                    }
                }
                else if(str == "%")
                {
                    if(double.TryParse(formulaList.LastOrDefault(), out double value))
                    {
                        formulaList.Add(str);
                    }
                }
                else
                {
                    if (str == "(" || str == ")" || (!formulaList.LastOrDefault().Contains("+")
                        && !formulaList.LastOrDefault().Contains("-") && !formulaList.LastOrDefault().Contains("x")
                        && !formulaList.LastOrDefault().Contains("÷") && !formulaList.LastOrDefault().Contains("(")))
                    {
                        formulaList.Add(str);
                    }
                }

                updateRtbFormula();
            }else if(str == "(")
            {
                formulaList.Add(str);
                updateRtbFormula();
            }
        }

        private void btnZero_Click(object sender, EventArgs e)
        {
            addFormulaList(0);
        }

        private void btnOne_Click(object sender, EventArgs e)
        {
            addFormulaList(1);
        }

        private void btnTwo_Click(object sender, EventArgs e)
        {
            addFormulaList(2);
        }

        private void btnThree_Click(object sender, EventArgs e)
        {
            addFormulaList(3);
        }

        private void btnFour_Click(object sender, EventArgs e)
        {
            addFormulaList(4);
        }

        private void btnFive_Click(object sender, EventArgs e)
        {
            addFormulaList(5);
        }

        private void btnSix_Click(object sender, EventArgs e)
        {
            addFormulaList(6);
        }

        private void btnSeven_Click(object sender, EventArgs e)
        {
            addFormulaList(7);
        }

        private void btnEight_Click(object sender, EventArgs e)
        {
            addFormulaList(8);
        }

        private void btnNine_Click(object sender, EventArgs e)
        {
            addFormulaList(9);
        }

        private void btnNegative_Click(object sender, EventArgs e)
        {
            if (formulaList.Count > 0 && double.TryParse(formulaList.LastOrDefault(), out double value))
            {
                if (formulaList.LastOrDefault().StartsWith("-"))
                {
                    formulaList[formulaList.Count - 1] = formulaList.LastOrDefault().Remove(0, 1);
                }else
                {
                    formulaList[formulaList.Count - 1] = formulaList.LastOrDefault().Insert(0, "-");
                }
            }

            updateRtbFormula();
        }

        private void btnComma_Click(object sender, EventArgs e)
        {
           addFormulaList(".");
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            addFormulaList("+");
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            addFormulaList("-");
        }

        private void btnMultiply_Click(object sender, EventArgs e)
        {
            addFormulaList("x");
        }

        private void btnDivide_Click(object sender, EventArgs e)
        {
            addFormulaList("÷");
        }

        private void btnPercent_Click(object sender, EventArgs e)
        {
            addFormulaList("%");
        }

        private void btnBreacket_Click(object sender, EventArgs e)
        {
            if(tbResult.Text.Length > 0 || formulaList.Count == 0 || formulaList.LastOrDefault().Contains("(")
                || formulaList.LastOrDefault().Contains("+") || (formulaList.LastOrDefault().Contains("-") 
                && !double.TryParse(formulaList.LastOrDefault(), out double value))
                || formulaList.LastOrDefault().Contains("x") || formulaList.LastOrDefault().Contains("÷"))
            {
                addFormulaList("(");
            }else if (formulaList.Count > 3)
            {
                int openBreaketCount = 0;
                int closeBreaketCount = 0;

                formulaList.ForEach(str =>
                {
                    if (str == "(")
                    {
                        openBreaketCount++;
                    }
                    else if (str == ")")
                    {
                        closeBreaketCount++;
                    }
                });

                if(openBreaketCount > closeBreaketCount)
                {
                    addFormulaList(")");
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            resetForm();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            tbResult.Text = "";

            if (formulaList.Count > 0)
            {
                if (double.TryParse(formulaList.LastOrDefault(), out double value))
                {
                    String formulaListLast = formulaList.LastOrDefault();
                    String formulaListLastRemove = formulaListLast.Remove(formulaListLast.Length - 1, 1);

                    if (formulaListLastRemove.Length > 0)
                    {
                        formulaList[formulaList.Count - 1] = formulaListLastRemove;
                    }
                    else
                    {
                        formulaList.RemoveAt(formulaList.Count - 1);
                    }
                }
                else
                {
                    formulaList.RemoveAt(formulaList.Count - 1);
                }
            }

            updateRtbFormula();
        }

        private double calculate(double a, String opt, double b = 0)
        {
            double result = 0;

            switch (opt)
            {
                case "+":
                    result = a + b;
                    break;
                case "-":
                    result = a - b;
                    break;
                case "x":
                    result = a * b;
                    break;
                case "÷":
                    if(b == 0)
                    {
                        throw new DivideByZeroException("Không thể chia cho 0 !\nVui lòng kiểm tra lại !");
                    }

                    result = a / b;
                    break;
                case "%":
                    result = a / 100;
                    break;
            }

            return result;
        }

        private String formulaShort(List<String> formulaShortList)
        {
            while (formulaShortList.Count > 1)
            {
                int indexOpt;
                double a;
                String opt;
                double b;

                if (formulaShortList.Contains("%"))
                {
                    indexOpt = formulaShortList.IndexOf("%");
                    a = double.Parse(formulaShortList[indexOpt - 1]);
                    opt = formulaShortList[indexOpt];

                    double res = calculate(a, opt);
                    formulaShortList[indexOpt - 1] = res.ToString();
                    formulaShortList.RemoveAt(indexOpt);
                }else
                {
                    if (formulaShortList.Contains("x") || formulaShortList.Contains("÷"))
                    {
                        if (formulaShortList.Contains("x") && !formulaShortList.Contains("÷"))
                        {
                            indexOpt = formulaShortList.IndexOf("x");
                        }
                        else if (!formulaShortList.Contains("x") && formulaShortList.Contains("÷"))
                        {
                            indexOpt = formulaShortList.IndexOf("÷");
                        }
                        else
                        {
                            int indexOptMul = formulaShortList.IndexOf("x");
                            int indexOptDev = formulaShortList.IndexOf("÷");

                            if (indexOptMul < indexOptDev)
                            {
                                indexOpt = indexOptMul;
                            }
                            else
                            {
                                indexOpt = indexOptDev;
                            }
                        }
                    }else
                    {
                        if (formulaShortList.Contains("+") && !formulaShortList.Contains("-"))
                        {
                            indexOpt = formulaShortList.IndexOf("+");
                        }
                        else if (!formulaShortList.Contains("+") && formulaShortList.Contains("-"))
                        {
                            indexOpt = formulaShortList.IndexOf("-");
                        }
                        else
                        {
                            int indexOptPlus = formulaShortList.IndexOf("+");
                            int indexOptMinus = formulaShortList.IndexOf("-");

                            if (indexOptPlus < indexOptMinus)
                            {
                                indexOpt = indexOptPlus;
                            }
                            else
                            {
                                indexOpt = indexOptMinus;
                            }
                        }
                    }

                    a = double.Parse(formulaShortList[indexOpt - 1]);
                    opt = formulaShortList[indexOpt];
                    b = double.Parse(formulaShortList[indexOpt + 1]);

                    double res = calculate(a, opt, b);
                    formulaShortList[indexOpt - 1] = res.ToString();
                    formulaShortList.RemoveRange(indexOpt, 2);
                }
            }


            return formulaShortList.LastOrDefault();
        }

        private void btnEqual_Click(object sender, EventArgs e)
        {
            try
            {
                // Cách 1: tự làm
                if (formulaList.Contains("(") || formulaList.Contains(")"))
                {
                    int openBreaketCount = 0;
                    int closeBreaketCount = 0;

                    formulaList.ForEach(str =>
                    {
                        if (str == "(")
                        {
                            openBreaketCount++;
                        }
                        else if (str == ")")
                        {
                            closeBreaketCount++;
                        }
                    });

                    if (openBreaketCount != closeBreaketCount)
                    {
                        throw new FormatException("Công thức sai định dạng !\nVui lòng kiểm tra lại !");
                    }
                }

                List<String> formulaListClone = formulaList.ToList();

                while (formulaListClone.Count > 1)
                {
                    if(formulaListClone.Contains("(") && formulaListClone.Contains(")"))
                    {
                        int indexOpenBreaket = formulaListClone.LastIndexOf("(");
                        int indexCloseBreaket = 0;
                        for(int i = indexOpenBreaket + 1; i < formulaListClone.Count; i++)
                        {
                            if(formulaListClone[i] == ")")
                            {
                                indexCloseBreaket = i;
                                break;
                            }
                        }

                        String resStr = formulaShort(formulaListClone.GetRange(indexOpenBreaket + 1,
                        indexCloseBreaket - indexOpenBreaket - 1));
                        formulaListClone[indexOpenBreaket] = resStr;
                        formulaListClone.RemoveRange(indexOpenBreaket + 1, indexCloseBreaket - indexOpenBreaket);
                    }
                    else
                    {
                        String resStr = formulaShort(formulaListClone);
                        formulaListClone.Clear();
                        formulaListClone.Add(resStr);
                    }
                }

                String result = formatNumber(formulaListClone.LastOrDefault());
                // Cách 1

                // Cách 2: dùng thư viện
                /*String expression = "";

                formulaList.ForEach(str =>
                {
                    if (str == "x")
                    {
                        expression += "*";
                    } else if (str == "÷")
                    {
                        expression += "/";
                    } else
                    {
                        expression += str;
                    }
                });

                DataTable table = new DataTable();
                table.Columns.Add("expression", typeof(string), expression);
                DataRow row = table.NewRow();
                table.Rows.Add(row);

                double result = double.Parse((string)row["expression"]);*/
                // Cách 2

                tbResult.Text = "= " + result;

                historyList.Add(rtbFormula.Text);
                historyList.Add(tbResult.Text);
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                formulaList.Clear();
                updateRtbFormula();
                Console.WriteLine(err);
            }
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            Form historyForm = new frmHistory(historyList);
            historyForm.StartPosition = FormStartPosition.CenterScreen; // Đặt vị trí chính giữa màn hình

            historyForm.ShowDialog();
        }
    }
}
