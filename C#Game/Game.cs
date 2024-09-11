// September 2024
// Yue Chen
using System;
using System.Drawing;
using System.Windows.Forms;

public class Game
{
    private float height = (float)Window.height;
    private float width = (float) Window.width;
    private Bee _bee = new Bee();
    private bool _creditBox = false;
    private Image background;
    private int score = 0;
    private Flower[] _flowers = new Flower[4];
    private int health = 3;
    private int Hightest = 0;

    public void Setup()
    {
        background = Image.FromFile("Images/Background.png");
        _bee.Setup();
        bool _flag = false;
        for (int i=0; i<4; i++) {
            _flowers[i] = new Flower();
            if (_flag)
            {
                _flowers[i].Setup("Images/Flower1.png",i);
            }
            else
            {
                _flowers[i].Setup("Images/Flower2.png", i);
            }
            _flag = !_flag;
        }
    }

    public void Update(float dt)
    {
        _bee._effect = false;
        _bee.Update(dt);
        if (_bee.y > (height + 96)) { health = 0; }
        foreach (Flower flower in _flowers) {
            // if a flower isn't collected by the bee
            if (!flower.Update(dt))
            {
                health--;
            }
            if (flower._visible && CalculateDistance(_bee.x, _bee.y, flower.x, flower.y) < 70.0f) {
                _bee._effect = true;
                score++;
                flower._visible = false;
            }
        }
    }

    public void Draw(Graphics g)
    {
        Font font = new Font("Times New Roman", 15);
        SolidBrush fontBrush = new SolidBrush(Color.Black);

        StringFormat format = new StringFormat();
        format.LineAlignment = StringAlignment.Center;
        format.Alignment = StringAlignment.Center;

        g.DrawImage(background, 0, 0, width, height);

        if(health > 0)
        {
            foreach (Flower flower in _flowers)
            {
                flower.Draw(g);
            }
            _bee.Draw(g);

            g.DrawString("Health:" + health.ToString() + "  Score:" + score.ToString() + 
                  "\npress SPACE to control", font, fontBrush,
                  (float)(width * 0.2),
                  (float)(height * 0.1),
                  format);
        }
        else
        {
            if (score > Hightest) { Hightest = score; }
            g.DrawString("Your Score is " + score.ToString()+ "\nPress R to Restart", font, fontBrush,
                  (float)(width * 0.5),
                  (float)(height * 0.5),
                  format);
            g.DrawString("Your Highest Score is " + Hightest.ToString(), font, fontBrush,
                  (float)(width * 0.2),
                  (float)(height * 0.2),
                  format);
        }

        if (_creditBox) { 
            g.DrawString("Yue Chen, 2025, Hungary Bee", font, fontBrush,
               (float)(width * 0.25),
               (float)(height * 0.93),
               format);
        }
    }

    public void MouseClick(MouseEventArgs mouse)
    {
        if (mouse.Button == MouseButtons.Left)
        {
            System.Console.WriteLine(mouse.Location.X + ", " + mouse.Location.Y);
        }
    }

    public void KeyDown(KeyEventArgs key)
    {
        if (key.KeyCode == Keys.Space)
        {
            _bee.KeyDown(key);
        }
        else if(key.KeyCode == Keys.Oemplus)
        {
            _creditBox = !_creditBox;
        }
        else if(key.KeyCode == Keys.R && health <=0)
        {
            health = 3;
            score = 0;
            _bee.Reset();
            for(int i=0;i<4;i++)
            {
                _flowers[i].Reset(i);
            }
        }
    }

    public float CalculateDistance(float x1, float y1, float x2, float y2)
    {
        float distance = (x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1);
        distance = (float)Math.Sqrt(distance);
        return distance;
    }
}
