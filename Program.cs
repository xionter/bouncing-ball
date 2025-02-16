using System;
using System.Text;
using System.Threading;

namespace Ball;

enum Pixel
{
    EMPTY = 0,
    FULL = 1
}

class Display
{
    public const int WIDTH = 128;
    public const int HEIGHT = 32;
    public const int FPS = 60; // Lowered FPS for visibility

    private Pixel[] display;
    
    public Display()
    {
        display = new Pixel[WIDTH * HEIGHT];
        Array.Fill(display, Pixel.EMPTY);
    }

    public void Fill(Pixel p)
    {
        for (int x = 0; x < HEIGHT * WIDTH; ++x)
        {
            display[x] = p;
        }
    }

    public void Print()
    {
        char[,] table = new char[2,2]{
            //0   1    top/bottom
            {' ','*'},   //0
            {',','C'}    //1
        };
        var row = new StringBuilder(WIDTH);
        
        for (int y = 0; y < HEIGHT / 2; ++y)
        {
            for (int x = 0; x < WIDTH; ++x)
            {
                var top = (int)display[(2 * y + 0) * WIDTH + x];
                var bottom = (int)display[(2 * y + 1) * WIDTH + x];
                row.Append(table[top, bottom]); 
            }
            Console.WriteLine(row.ToString());
            row.Clear();
        }
    }

    public void DrawBall(float cX, float cY, float r)
    {
        var beginX = cX - r;
        var beginY = cY - r;
        var endX = cX + r;
        var endY = cY + r;

        for (int y = (int)beginY; y <= endY; ++y)
        {
            for (int x = (int)beginX; x <= endX; ++x)
            {
                var dY = cY - (y + 0.5f);
                var dX = cX - (x + 0.5f);
                
                if (dX * dX + dY * dY <= r * r)
                {
                    if (0 <= y && y < HEIGHT && 0 <= x && x < WIDTH)
                    {
                        display[y * WIDTH + x] = Pixel.FULL;
                    }
                }
            }
        }
    }

    public void Clear()
    {
        Console.Write("\x1b[H");
    }
}

class Program
{
    static void Main()
    {
        Console.CursorVisible = false;
        var display = new Display();

        display.Clear();

        float radius = 8.0f;
        float centerY = -radius;
        float centerX = -radius; 
        float velY = 0.5f; 
        float velX = 0.5f; 
        float accY = 0.1f;

        for (int i = 0; i < 10000; ++i)
        {
            centerX += velX;
            centerY += velY;
            velY += accY;
            
            if (centerY > Display.HEIGHT - radius)
            {
                centerY = Display.HEIGHT - radius;
                velY = -velY * 0.65f; 
            }

            display.Fill(Pixel.EMPTY);
            display.DrawBall(centerX, centerY, radius);
            display.Print();
            display.Clear();
            Thread.Sleep(1000 / Display.FPS);
        }
    }
}
