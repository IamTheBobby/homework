using System;
using System.Windows.Forms;
using System.Drawing;
public class WinInOut : Form
{
    TextBox txt1 = new TextBox();
    TextBox txt2 = new TextBox();
    Button btn = new Button();
    Label lbl = new Label();

    public void init()
    {
        this.Controls.Add(txt1);
        this.Controls.Add(txt2);
        this.Controls.Add(btn);
        this.Controls.Add(lbl);
        txt1.Dock = System.Windows.Forms.DockStyle.Left;
        txt2.Dock = System.Windows.Forms.DockStyle.Right;
        btn.Dock = System.Windows.Forms.DockStyle.Top;
        lbl.Dock = System.Windows.Forms.DockStyle.Bottom;
        btn.Text = "求两数乘积";
        lbl.Text = "用于显示结果的标签";
        this.Size = new Size(300, 120);

        btn.Click += new System.EventHandler(this.button1_Click);
    }

    public void button1_Click(object sender, EventArgs e)
    {
        string s1 = txt1.Text;
        string s2 = txt2.Text;
        double d1 = Double.Parse(s1);
        double d2 = Double.Parse(s2);
        double sq = d1 * d2;
        lbl.Text = d1 + "和" + d2 + "的乘积是:" + sq;
    }

    static void Main()
    {
        WinInOut f = new WinInOut();
        f.Text = "WinInOut";
        f.init();
        Application.Run(f);
    }
}
