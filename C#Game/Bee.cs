/* author: Yue Chen
 * date: Sept.12, 2024
*/
using System;
using System.Drawing;
using System.Windows.Forms;

public class Bee
{
    private Image bee, effect;
    private float velocity;
    public float x;
    public float y;
    public bool _effect = false;

    public void Setup()
    {
        velocity = 0.0f;
        x = 50.0f;
        y = (float)(Window.height * 0.5);
        effect = Image.FromFile("Images/Effect.png");
        bee = Image.FromFile("Images/Bee.png");
    }

    public void Update(float dt)
    {
        // the velocity follows the gravity rule
        velocity += dt * 98.0f ;
        y += velocity * dt;
    }

    public void Draw(Graphics g)
    {
        g.DrawImage(bee, x, y, 96, 96);
        if (_effect)
        {
            g.DrawImage(effect, x, y, 96, 96);
        }
    }

    public void MouseClick(MouseEventArgs mouse)
    {
       
    }

    public void KeyDown(KeyEventArgs key)
    {
        if (key.KeyCode == Keys.Space)
        {
            // the bee fly up and the velocity is reset
            velocity = 20.0f;
            y -= 20.0f; 
        }
    }

    public void Reset()
    {
        velocity = 0.0f;
        x = 50.0f;
        y = (float)(Window.height * 0.5);
    }
}
