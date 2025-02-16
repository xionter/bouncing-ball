using System.Text;
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
    public const int FPS = 60;

    private Pixel[] display;
    public Display()
    {
        display = new Pixel[WIDTH * HEIGHT];
        Array.Fill(display, Pixel.EMPTY);
    }

    public void Fill(Pixel p)
    {
        for(int x = 0; x < HEIGHT * WIDTH; ++x)
        {
            display[x] = p;
        }
    }

    public void Print()
    {
        char[,] table = new char[2,2]{
        //    0   1    top/bottom
            {' ','*'},   //0
            {',','C'}    //1
        };
        var frame = new StringBuilder(WIDTH * HEIGHT);
        for(int y = 0; y < HEIGHT/2; ++y)
        {
            for(int x = 0; x < WIDTH; ++x)
            {
                var top = (int)display[(2*y + 0)*WIDTH + x]; 
                var bottom = (int)display[(2*y + 1)*WIDTH  + x];
                frame.Append(table[top, bottom]);
            }
            frame.Append("\n");
        }
        Console.Write(frame);
    }
    
    public void DrawBall(float cX, float cY, float r)
    {
        var beginX = cX - r;
        var beginY = cY - r;
        var endX = cX + r;
        var endY = cY + r;
        for(float y = beginY; y <= endY; ++y)
        {
            for(float x = beginX; x <= endX; ++x)
            {
                var dY = cY - y - 0.5f;
                var dX = cX - x - 0.5f;
                if(dX*dX + dY*dY <= r*r){
                    if(0 <= y && y < HEIGHT && 0 <= x && x < WIDTH)
                    {
                        display[(int)(y*WIDTH + x)] = Pixel.FULL;
                    }
                }
            }
        }
    }

    public void CheckCollision(ref float centerX, ref float centerY, float radius,ref float velX, ref float velY)
    {
        if(centerY+radius - Display.HEIGHT >= 0.0f)
        {
            velY = -velY;
            centerY = Display.HEIGHT - radius;
        }
        else if(centerY-radius <= 0.0f)
        {
            velY = -velY;
            centerY = radius; 
        }
        else if(centerX+radius - Display.WIDTH >= 0.0f)
        {
            velX = -velX;
            centerX = Display.WIDTH - radius;
        }
        else if(centerX-radius <= 0.0f)
        {
            velX = -velX;
            centerX = radius;
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
        var centerY = (float)Display.HEIGHT/2;
        var centerX = 32.0f; 
        var radius = 8.0f;
        var velY = 1.0f;
        var velX = 1.0f;

        for(int i = 0; i < 10000; ++i){
            centerX+=velX;
            centerY+=velY;

            display.CheckCollision(ref centerX, ref centerY,radius,ref velX,ref velY);

            display.Clear();
            display.DrawBall(centerX, centerY, radius);
            display.Print();

            display.Fill(Pixel.EMPTY);

            Thread.Sleep(1000/Display.FPS);
        }
    }
}
