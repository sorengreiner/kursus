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
    int image_width = 1024;
    int image_height = 1024;
    const int scale = 2;

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
        this.pictureBox1.Size = new System.Drawing.Size(image_width/scale, image_height/scale);

        this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
        this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);

        this.ClientSize = new System.Drawing.Size(image_width/scale + 2, image_height/scale + 2);
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
        g.DrawImage(myBitmap, 0, 0, myBitmap.Width/scale, myBitmap.Height/scale);
        Refresh();
    }

    private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
    {
        MouseEventArgs me = (MouseEventArgs)e;
        Point coordinates = me.Location;

        int h = myBitmap.Height/scale;
        int w = myBitmap.Width/scale;
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
        const int max_iterations = 1024;

        float r0 = 45;
        float g0 = 210;
        float b0 = 10;

        float r1 = 200;
        float g1 = 100;
        float b1 = 255;

        // Generate palette
        int[] red = new int[256];
        int[] green = new int[256];
        int[] blue = new int[256];
        for(int i = 0; i < 128; i++)
        {
            float t = (float)i / 127;
            red[i] = (int)((r1 - r0) * t + r0);
            green[i] = (int)((g1 - g0) * t + g0);
            blue[i] = (int)((b1 - b0) * t + b0);

            red[255 - i] = red[i];
            green[255 - i] = green[i];
            blue[255 - i] = blue[i];
        }

        // Render image
        double du = (1.0 / w) * zoom;
        double dv = (1.0 / h) * zoom;
        double v = - zoom / 2.0 + vc;
        for (int y = 0; y < h; y++, v += dv)
        {
            double u = - zoom / 2.0 + uc;
            for (int x = 0; x < w; x++, u += du)
            {
                double zr = 0;
                double zi = 0;
                int i = 0;
                while ((Math.Abs(zr) < 2.0) && (Math.Abs(zi) < 2.0) && (i < max_iterations))
                {
                    double zr_n = zr * zr - zi * zi + u;
                    double zi_n = 2 * zi * zr + v;
                    zr = zr_n;
                    zi = zi_n;
                    i++;
                }

                if(i < max_iterations)
                {
                    int color_index = i % 256;
                    myBitmap.SetPixel(x, y, Color.FromArgb(red[color_index], green[color_index], blue[color_index]));
                }
                else
                {
                    myBitmap.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                }
            }
        }
    }

    public static void Main()
    {
        System.Windows.Forms.Application.Run(new MyForm());
    }
}

