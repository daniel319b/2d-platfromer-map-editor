using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Paths
{
    class FormsManager
    {
        static List<Form> forms = new List<Form>();
      
        public static void AddForm(Form form)
        {
            forms.Add(form);
        }

        public static void RemoveForm(Form form)
        {
            form.Alive = false;
        }

        public static void Update()
        {
            List<Form> remove = new List<Form>(forms.Count);
            foreach (Form f in forms)
            {
                f.Update();
                if (f.Alive == false) 
                    remove.Add(f);     
            }
            for (int i = 0; i < remove.Count; i++)
            {
                forms.RemoveAt(i);
                remove[i] = null;
            }
            remove.Clear();
        }

        public static void Draw(SpriteBatch sb)
        {
            foreach (Form f in forms)
                f.Draw(sb);
        }

        public static Form[] GetForms()
        {
            return forms.ToArray();
        }
    }
}
