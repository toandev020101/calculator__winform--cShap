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

        /**
        * Chức năng: Định dạng chuỗi số
        * 
        * Phương thức này nhận vào một chuỗi str chứa số và thực hiện định dạng
        * chuỗi số đó bằng cách thêm dấu "." (chấm) phân cách hàng nghìn và thay thế
        * dấu "." (chấm) bằng dấu "," (phẩy) cho phần thập phân (nếu có).
        * 
        * @param str - Chuỗi chứa số cần định dạng.
        * @return Chuỗi đã được định dạng.
        */

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

        /**
        * Chức năng: Cập nhật nội dung của RichTextBox "rtbFormula" dựa trên danh sách công thức "formulaList".
        * 
        * Phương thức này thực hiện việc cập nhật nội dung của RichTextBox "rtbFormula" dựa trên các chuỗi công thức
        * trong danh sách "formulaList". Nó thêm dấu cách vào các phép toán và định dạng các số trong danh sách,
        * sau đó đổi màu sắc cho các thành phần trong công thức.
        */

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

                // đổi màu chuỗi
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
                        || rtbFormulaText[i] == 'x' || rtbFormulaText[i] == '÷') // đổi màu phép toán
                    {
                        rtbFormula.Select(i, 1);
                        rtbFormula.SelectionColor = colorMath;
                    }else if (rtbFormulaText[i] == '(') // đổi màu ngoặc
                    {
                        rtbFormula.Select(i, 1);
                        rtbFormula.SelectionColor = colorList[indexColor];
                        colorUsedList.Add(colorList[indexColor]);
                        indexColor++;
                    }else if (rtbFormulaText[i] == ')') // đổi màu ngoặc
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

        /**
         * Chức năng: Thêm một số hoặc cộng tiếp vào danh sách công thức "formulaList".
         * 
         * Phương thức này cho phép thêm một số hoặc cộng tiếp số vào danh sách công thức "formulaList".
         * Nếu danh sách "tbResult" không rỗng, nó sẽ được đặt lại bằng cách gọi phương thức "resetForm".
         * Sau đó, nếu số cuối cùng trong "formulaList" có thể chuyển thành số thực, số mới sẽ được cộng tiếp
         * vào chuỗi số đó. Nếu số cuối cùng là 0 và không chứa dấu thập phân ".", số mới sẽ ghi đè lên số 0.
         * Nếu không có số trong danh sách, số mới sẽ được thêm vào danh sách.
         * Cuối cùng, phương thức sẽ gọi phương thức "updateRtbFormula" để cập nhật nội dung RichTextBox.
         * 
         * @param number - Số cần thêm hoặc cộng tiếp vào danh sách công thức.
         */

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

        /**
         * Chức năng: Thêm một chuỗi hoặc ký tự vào danh sách công thức "formulaList".
         * 
         * Phương thức này cho phép thêm một chuỗi hoặc ký tự vào danh sách công thức "formulaList".
         * Nếu nội dung của "tbResult" không rỗng, phương thức sẽ đặt lại form bằng cách gọi "resetForm".
         * Nếu danh sách "formulaList" không rỗng, phương thức sẽ kiểm tra chuỗi được thêm và thực hiện
         * các kiểm tra để quyết định liệu chuỗi đó có phù hợp để được thêm vào danh sách hay không.
         * Nếu chuỗi là dấu thập phân "." và số cuối cùng trong danh sách có thể chuyển thành số thực và
         * không chứa dấu thập phân, chuỗi "." sẽ được thêm vào số cuối cùng.
         * Nếu chuỗi là dấu phần trăm "%", nó sẽ được thêm vào danh sách nếu số cuối cùng có thể chuyển thành số thực.
         * Nếu chuỗi không phải là dấu ".", dấu phần trăm "%" hoặc dấu ngoặc "(", và số cuối cùng trong danh sách
         * không chứa các phép toán hoặc dấu ngoặc, chuỗi sẽ được thêm vào danh sách.
         * Nếu danh sách rỗng và chuỗi là dấu ngoặc "(", chuỗi sẽ được thêm vào danh sách.
         * Cuối cùng, phương thức sẽ gọi "updateRtbFormula" để cập nhật nội dung RichTextBox.
         * 
         * @param str - Chuỗi hoặc ký tự cần thêm vào danh sách công thức.
         */

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

        /**
         * Chức năng: Xử lý sự kiện khi nút "Negative" được nhấn.
         * 
         * Phương thức này được gọi khi nút "Negative" được nhấn. Nếu nội dung của "tbResult" không rỗng,
         * phương thức sẽ đặt lại form bằng cách gọi "resetForm".
         * Nếu danh sách "formulaList" không rỗng và số cuối cùng trong danh sách có thể chuyển thành số thực,
         * phương thức sẽ thực hiện chuyển đổi giữa số dương và số âm bằng cách thêm hoặc xóa dấu trừ "-" từ số cuối cùng.
         * Cuối cùng, phương thức sẽ gọi "updateRtbFormula" để cập nhật nội dung RichTextBox.
         * 
         * @param sender - Đối tượng gửi sự kiện (nút "Negative").
         * @param e - Đối tượng chứa thông tin về sự kiện.
         */

        private void btnNegative_Click(object sender, EventArgs e)
        {
            if(tbResult.Text.Length > 0)
            {
                resetForm();
            }else if (formulaList.Count > 0 && double.TryParse(formulaList.LastOrDefault(), out double value))
            {
                if (formulaList.LastOrDefault().StartsWith("-"))
                {
                    formulaList[formulaList.Count - 1] = formulaList.LastOrDefault().Remove(0, 1);
                }else
                {
                    formulaList[formulaList.Count - 1] = formulaList.LastOrDefault().Insert(0, "-");
                }

                updateRtbFormula();
            }
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

        /**
         * Chức năng: Xử lý sự kiện khi nút "Bracket" được nhấn.
         * 
         * Phương thức này được gọi khi nút "Bracket" được nhấn. Nó thực hiện kiểm tra
         * các điều kiện để quyết định xem nút "Bracket" nên thêm dấu mở ngoặc "("
         * hoặc dấu đóng ngoặc ")" vào danh sách công thức "formulaList". Các điều kiện
         * kiểm tra bao gồm:
         * 1. Nếu nội dung "tbResult" không rỗng.
         * 2. Nếu danh sách "formulaList" trống.
         * 3. Nếu số cuối cùng trong danh sách "formulaList" đã chứa dấu "(".
         * 4. Nếu số cuối cùng trong danh sách "formulaList" đã chứa phép toán cộng "+".
         * 5. Nếu số cuối cùng trong danh sách "formulaList" đã chứa phép toán trừ "-" và không thể chuyển thành số thực.
         * 6. Nếu số cuối cùng trong danh sách "formulaList" đã chứa phép toán nhân "x".
         * 7. Nếu số cuối cùng trong danh sách "formulaList" đã chứa phép toán chia "÷".
         * 
         * Nếu một trong các điều kiện trên đúng, phương thức sẽ thêm dấu mở ngoặc "(" vào danh sách.
         * Nếu không, nếu số lượng dấu mở ngoặc "(" đã xuất hiện trong danh sách nhiều hơn số lượng dấu
         * đóng ngoặc ")", nút "Bracket" sẽ thêm dấu đóng ngoặc ")" vào danh sách để cân bằng ngoặc đó.
         * 
         * @param sender - Đối tượng gửi sự kiện (nút "Bracket").
         * @param e - Đối tượng chứa thông tin về sự kiện.
         */

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

        /**
         * Chức năng: Xử lý sự kiện khi nút "Remove" được nhấn.
         * 
         * Phương thức này được gọi khi nút "Remove" được nhấn. Nó thực hiện các thao tác sau:
         * 1. Xóa nội dung của "tbResult".
         * 2. Kiểm tra nếu danh sách công thức "formulaList" không rỗng:
         *    a. Nếu số cuối cùng trong danh sách có thể chuyển thành số thực, phương thức sẽ thực hiện các bước sau:
         *       i.   Lấy số cuối cùng trong danh sách.
         *       ii.  Xóa ký tự cuối cùng của số đó.
         *       iii. Nếu số sau khi xóa ký tự cuối cùng vẫn có độ dài lớn hơn 0, số đó được cập nhật lại vào danh sách.
         *       iv.  Ngược lại, nếu số sau khi xóa ký tự cuối cùng có độ dài bằng 0, số đó được xóa khỏi danh sách.
         *    b. Nếu số cuối cùng trong danh sách không thể chuyển thành số thực, số đó được xóa khỏi danh sách.
         * 3. Cuối cùng, phương thức gọi "updateRtbFormula" để cập nhật nội dung RichTextBox.
         * 
         * @param sender - Đối tượng gửi sự kiện (nút "Remove").
         * @param e - Đối tượng chứa thông tin về sự kiện.
         */

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

        /**
        * Chức năng: Rút gọn danh sách công thức thành một giá trị kết quả duy nhất.
        * 
        * Phương thức này nhận vào một danh sách các chuỗi công thức "formulaShortList" và rút gọn chúng
        * thành một giá trị duy nhất bằng cách thực hiện các phép tính theo đúng thứ tự ưu tiên:
        * 1. Ưu tiên tính phần trăm (%) trước, nếu tồn tại.
        * 2. Sau đó, tính toán các phép nhân (x) và chia (÷) theo thứ tự xuất hiện.
        * 3. Cuối cùng, tính toán các phép cộng (+) và trừ (-) theo thứ tự xuất hiện.
        * 
        * Phương thức sử dụng một vòng lặp while để rút gọn danh sách cho đến khi chỉ còn một giá trị duy nhất.
        * Trong quá trình rút gọn, nó sẽ thực hiện các phép tính và cập nhật danh sách "formulaShortList".
        * Cuối cùng, giá trị duy nhất cuối cùng trong danh sách sẽ được trả về.
        * 
        * @param formulaShortList - Danh sách các chuỗi công thức cần rút gọn.
        * @return Giá trị kết quả sau khi rút gọn danh sách công thức.
        */

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

        /**
         * Chức năng: Xử lý sự kiện khi nút "Equal" được nhấn.
         * 
         * Phương thức này được gọi khi nút "Equal" được nhấn. Nó thực hiện các thao tác sau:
         * 1. Kiểm tra định dạng của công thức:
         *    a. Đảm bảo số lượng dấu mở ngoặc "(" và đóng ngoặc ")" phù hợp. Nếu không, sẽ ném ngoại lệ FormatException.
         * 2. Tạo một bản sao của danh sách công thức "formulaList" để rút gọn và tính toán giá trị.
         * 3. Trong vòng lặp while, phương thức thực hiện rút gọn danh sách công thức cho đến khi chỉ còn một giá trị duy nhất.
         * 4. Định dạng số cuối cùng và hiển thị kết quả trong "tbResult".
         * 5. Lưu công thức và kết quả vào danh sách lịch sử "historyList".
         * 6. Xử lý ngoại lệ và hiển thị thông báo lỗi nếu có lỗi xảy ra.
         * 
         * @param sender - Đối tượng gửi sự kiện (nút "Equal").
         * @param e - Đối tượng chứa thông tin về sự kiện.
         */

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
