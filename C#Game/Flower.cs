using System;
using System.Drawing;
using System.Windows.Forms;

public class Flower
{
    private Image flower;
    public float x;
    public float y;
    public bool _visible;
    private float velocity;

    public void Setup(string path, int index)
    {
        _visible = true; 
        velocity = 120.0f;
        flower = Image.FromFile(path);
        x = Window.width + index * 200;
        y = (float)(index * 90 + Window.height*0.15);
    }

    public bool Update(float dt)
    {
        bool _collect = true;
        x -= dt * velocity;
        if (x < 0) {
            _collect = !_visible;
            _visible = true;
            Random rnd = new Random();
            x = Window.width +  175;
            y = (float)(rnd.Next((int)(Window.height * 0.8)) + Window.height * 0.1);
        }
        velocity += dt * 2;
        return _collect;
    }

    public void Draw(Graphics g)
    {
        if (_visible)
        {
            g.DrawImage(flower, x, y, 80, 80);
        }
    }

    public void MouseClick(MouseEventArgs mouse)
    {

    }

    public void KeyDown(KeyEventArgs key)
    {
        if (key.KeyCode == Keys.Space)
        {
           
            y -= 20.0f;
        }
    }

    public void Reset(int index)
    {
        _visible = true;
        velocity = 120.0f;
        x = Window.width + index * 200;
        y = (float)(index * 90 + Window.height * 0.15);
    }
}
