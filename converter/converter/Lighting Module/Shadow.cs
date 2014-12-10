using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Convert;

namespace Lighting_Module
{
    class Shadow
    {
        string[] priority_list = { "chand", "pit", "brazier", "lamp" , "torch" ,"" };

        Dictionary<string, int> light_counts = new Dictionary<string, int>();

        public void count(List<TES3.REFR> references)
        {
            
            foreach (TES3.REFR r in references)
            {

                if (TypeIndex.getInstance().get(r.editor_id) == TypeIndex.TYPE.LIGH)
                {
                    if (!light_counts.ContainsKey(r.editor_id))
                    {
                        light_counts.Add(r.editor_id, 1);
                    }

                    light_counts[r.editor_id] += 1;
                }

            }
        }

        public void mark_shadow_lights(List<TES3.REFR> references)
        {

            int count = 0;

            foreach (string lght in priority_list)
            {
                if (count >= 4)
                {
                    return;
                }
                foreach (TES3.REFR r in references)
                {
                    if (TypeIndex.getInstance().get(r.editor_id) != TypeIndex.TYPE.LIGH)
                    {
                        continue;
                    }

                    if (count >= 4)
                    {
                        return;
                    }
                    if (r.editor_id.Contains(lght))
                    {
                        r.editor_id = r.editor_id + "_shadow";
                        count++;
                    }
                }

            }

            

        }

        public void getMin()
        {
            if (light_counts.Values == null)
            {
                return;
            }

            int[] counts = light_counts.Values.ToArray();

            if (counts.Length > 0)
            {
                Array.Sort(counts);
                Utility.Log.info(counts[0]);

                if (counts[0] > 4)
                {
                    Utility.Log.confirm("Oh No");
                }
            }            
        }

    }
}
