using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

namespace pavilion
{
    public class Demo_control : MonoBehaviour
    {
        public int index;
        public GameObject[] gameobjects;

        public AudioSource audio_source;
        public AudioClip ka;

        public void on_next_btn()
        {
            this.index++;
            if (this.index >= this.gameobjects.Length)
                this.index = 0;

            this.change_to_index();
        }

        public void on_previous_btn()
        {
            this.index--;
            if (this.index < 0)
                this.index = this.gameobjects.Length - 1;

            this.change_to_index();
        }

        private void change_to_index()
        {
            this.audio_source.PlayOneShot(this.ka);

            for (int i = 0; i < this.gameobjects.Length; i++)
            {
                if (this.index == i)
                {
                    this.gameobjects[i].SetActive(true);
                }
                else
                {
                    this.gameobjects[i].SetActive(false);
                }
            }
        }



        public void play_btn_sound()
        {
            if (this.ka != null)
                this.audio_source.PlayOneShot(this.ka, 0.5f);
        }
    }
}