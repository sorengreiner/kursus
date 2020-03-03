using System;
using System.Drawing;
using System.Windows.Forms;


public class MyForm : System.Windows.Forms.Form
{
    Bitmap myBitmap;
    Pen blackPen;
    double zoom = 4.0f;
    double uc = 0.0f;
    double vc = 0.0f;
    int image_width = 512;
    int image_height = 512;

    private System.Windows.Forms.PictureBox pictureBox1;

    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    public MyForm()
    {
        this.pictureBox1 = new System.Windows.Forms.PictureBox();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
        this.SuspendLayout();

        this.pictureBox1.Location = new System.Drawing.Point(1, 1);
        this.pictureBox1.Size = new System.Drawing.Size(image_width, image_height);

        this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
        this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);

        this.ClientSize = new System.Drawing.Size(image_width + 2, image_height + 2);
        this.Controls.Add(this.pictureBox1);

        ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
        this.ResumeLayout(false);

        this.Text = "Mandelbrot by Soren Greiner";

        blackPen = new Pen(Color.White, 1);
        myBitmap = new Bitmap(image_width, image_height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

        Render();
    }

    private void pictureBox1_Paint(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.DrawImage(myBitmap, 0, 0, myBitmap.Width, myBitmap.Height);
        Refresh();
    }

    private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
    {
        MouseEventArgs me = (MouseEventArgs)e;
        Point coordinates = me.Location;

        int h = myBitmap.Height;
        int w = myBitmap.Width;
        if(e.Button == MouseButtons.Right)
        {
            zoom = zoom * 1.5f;
        }
        else
        {
            zoom = zoom / 1.5f;
            uc = ((double)coordinates.X / w) * zoom - zoom / 2f + uc;
            vc = ((double)coordinates.Y / h) * zoom - zoom / 2f + vc;
        }
        Render();

        this.Invalidate();
    }
 
    private void Render()
    {
        int h = myBitmap.Height;
        int w = myBitmap.Width;

        int red = 0;
        int green = 0;
        int blue = 0;

        float r0 = 0;
        float g0 = 0;
        float b0 = 40;

        float r1 = 200;
        float g1 = 180;
        float b1 = 0;

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                double u = (((double)x) / w) * zoom - zoom / 2f + uc;
                double v = (((double)y) / h) * zoom - zoom / 2f + vc;
                double cr = u;
                double ci = v;
                double zr = 0;
                double zi = 0;

                int i = 0;
                while (true)
                {
                    double zr_n = zr * zr - zi * zi + cr;
                    double zi_n = 2 * zi * zr + ci;
                    zr = zr_n;
                    zi = zi_n;

                    double m = zr * zr + zi * zi;
                    if (m > 4)
                    {
                        int iterations = i % 256;
                        if(iterations >= 128)
                        {
                            iterations = 255 - iterations;
                        }
                        float t = ((float)(iterations % 128)) / 127;
                        red = (int)((r1 - r0) * t + r0);
                        green = (int)((g1 - g0) * t + g0);
                        blue = (int)((b1 - b0) * t + b0);
                        break;
                    }
                    if (i > 1000)
                    {
                        red = 0;
                        green = 0;
                        blue = 0;
                        break;
                    }
                    i++;
                }

                myBitmap.SetPixel(x, y, Color.FromArgb(red, green, blue));
            }
        }
    }

    public static void Main()
    {
        System.Windows.Forms.Application.Run(new MyForm());
    }
}

