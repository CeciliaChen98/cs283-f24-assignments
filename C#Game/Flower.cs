/* author: Yue Chen
 * date: Sept.12, 2024
*/

using System;
using System.Drawing;
using System.Windows.Forms;

public class Flower
{
    private Image flower; // stores the image
    public float x;
    public float y;
    public bool _visible; // if the flower is collected by the bee
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
            // if a flower reach the end but it is still visible
            // it means the flower isn't collected by the bee
            _collect = !_visible;
            _visible = true;
            Random rnd = new Random();
            x = Window.width +  175;
            y = (float)(rnd.Next((int)(Window.height * 0.8)) + Window.height * 0.1);
        }
        // velocity inceases along the time
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


    public void KeyDown(KeyEventArgs key)
    {

    }

    public void Reset(int index)
    {
        // reset all the data
        _visible = true;
        velocity = 120.0f;
        x = Window.width + index * 200;
        y = (float)(index * 90 + Window.height * 0.15);
    }
}
