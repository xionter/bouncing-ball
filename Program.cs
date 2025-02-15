using System.Text;
namespace Ball;


enum Pixel
{
    EMPTY = 0,
    FULL = 1
}

class Display
{
    public const int WIDTH = 64;
    public const int HEIGHT = 64;

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
            {',','i'}    //1
        };

        var row = new StringBuilder(WIDTH);
        for(int y = 0; y < HEIGHT/2; ++y)
        {
            for(int x = 0; x < WIDTH; ++x)
            {
                var top = (int)display[(2*y + 0)*WIDTH + x]; 
                var bottom = (int)display[(2*y + 1)*WIDTH  + x];
                row.Insert(x, table[top, bottom]);
            }
            Console.WriteLine(row);
            row.Clear();
        }
    }
    
    public void DrawBall(int cX, int cY, int r)
    {
        var beginX = cX - r;
        var beginY = cY - r;
        var endX = cX + r;
        var endY = cY + r;
        for(int y = beginY; y <= endY; ++y)
        {
            for(int x = beginX; x <= endX; ++x)
            {
                var dY = cY - y;
                var dX = cX - x;
                if(dX*dX + dY*dY <= r*r){
                    if(0 <= y && y < HEIGHT && 0 <= x&& x < WIDTH)
                    {
                        display[y*WIDTH + x] = Pixel.FULL;
                    }
                }
            }
        }
    }
    public void Clear()
    {
        //TODO
    }

}




class Program
{
    static void Main()
    {
        var display = new Display();
        var centerY = Display.HEIGHT/2;
        var centerX = Display.WIDTH/2;
        var radius = 16;
        var velY = 5;
        var velX = 0;
        for(int i = 0; i < 10; ++i){
            centerX+=velX;
            centerY+=velY;
            display.DrawBall(centerX, centerY, radius);
            display.Print();
            Console.WriteLine("----------------------------------");
            display.Fill(Pixel.EMPTY);
        }
    }
}
