using System;
using System.Text;
using System.Threading;

namespace Ball;

enum Pixel
{
    EMPTY = 0,
    FULL = 1
}

class Vector
{
    public float x;
    public float y;
    
    public Vector(float x, float y)
    {
        this.x = x; this.y = y;
    }
    public Vector(float num)
    {
        this.x = num; this.y = num;
    }
    public void Add (Vector A)
    {
        x += A.x; y += A.y;
    }
    public void Sub (Vector A)
    {
        x -= A.x; y -= A.y;
    }
}
class Display
{
    public const int WIDTH = 256;
    public const int HEIGHT = 96;
    public const int FPS = 60; 

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
            {',','O'}    //1
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

    public void DrawBall(Vector center, float r)
    {
        var begin = new Vector(center.x - r, center.y - r);
        var end = new Vector(center.x + r, center.y + r);

        for (int y = (int)begin.y; y <= end.y; ++y)
        {
            for (int x = (int)begin.x; x <= end.x; ++x)
            {
                var dY = center.y - (y + 0.5f);
                var dX = center.x - (x + 0.5f);
                
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

        float radius = Display.HEIGHT / 4.0f; 
        var center = new Vector(radius);
        var vel = new Vector(2.5f);

        float accY = 0.1f;

        while(true)
        {
            center.Add(vel);
            vel.Add(new Vector(0, accY));
            
            if (center.y > Display.HEIGHT - radius)
            {
                center.y = Display.HEIGHT - radius;
                vel.y = -vel.y * 0.9f; 
            }

            if (center.y < radius) 
            {
                center.y = radius;
                vel.y = -vel.y * 0.9f; 
            }
            if (center.x > Display.WIDTH - radius)
            {
                center.x = Display.WIDTH - radius;
                vel.x = -vel.x * 0.8f;
            }

            if (center.x < radius) 
            {
                center.x = radius;
                vel.x = -vel.x * 0.8f;
            }


            display.Fill(Pixel.EMPTY);
            display.DrawBall(center, radius);
            display.Print();
            display.Clear();
            Thread.Sleep(1000 / Display.FPS);
        }
    }
}
