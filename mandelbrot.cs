using System;
using System.Drawing;
using System.Windows.Forms;


public class MyForm : System.Windows.Forms.Form
{
    int image_width = 512;
    int image_height = 512;
    Bitmap myBitmap;
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
        // 
        // pictureBox1
        // 
        this.pictureBox1.Location = new System.Drawing.Point(2, 2);
        this.pictureBox1.Name = "pictureBox1";
        this.pictureBox1.Size = new System.Drawing.Size(image_width, image_height);
        this.pictureBox1.TabIndex = 0;
        this.pictureBox1.TabStop = false;
        this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
        // 
        // Form1
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(image_width + 4, image_height + 4);
        this.Controls.Add(this.pictureBox1);
        this.Name = "Form1";
        this.Text = "Form1";
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
        this.ResumeLayout(false);

        this.Text = "Mandelbrot by Soren Greiner";

        myBitmap = new Bitmap(image_width, image_height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

        int h = myBitmap.Height;
        int w = myBitmap.Width;

        float zoom = 0.5f;
        int red = 0;
        int green = 0;
        int blue = 0;

        float r0 = 0;
        float g0 = 0;
        float b0 = 50;

        float r1 = 255;
        float g1 = 200;
        float b1 = 0;

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                float u = ((float)x) / w + 0.2f;
                float v = ((float)y) / h + 0.2f;
                u = u * zoom;
                v = v * zoom;
                float cr = u * 4.0f - 2.0f;
                float ci = v * 4.0f - 2.0f;
                float zr = 0;
                float zi = 0;

                // perform square
                // (zr + zi*i)*(zr + zi*i) = zr*zr - zi*zi + 2*zi*i
                // zr = zr*zr - zi*zi
                // zi = 2*zi

                int i = 0;
                while (true)
                {
                    float zr_n = zr * zr - zi * zi + cr;
                    float zi_n = 2 * zi * zr + ci;
                    zr = zr_n;
                    zi = zi_n;

                    float m = zr * zr + zi * zi;
                    if (m > 4)
                    {
                        float t = ((float)(i % 32)) / 31;
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

    private void pictureBox1_Paint(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.DrawImage(myBitmap, 0, 0, myBitmap.Width, myBitmap.Height);
    }

    public static void Main()
    {
        System.Windows.Forms.Application.Run(new MyForm());
    }
 
}

