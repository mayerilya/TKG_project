using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextReplasment_class
{
    class Road_price_drop
    {

        //public List<road_data_dmg> drp_lst;
        public double dmg_sum = 0;

        public void clear_list()
        {
            dmg_sum = 0;
        }

        public double evaluate_one_part(int traffic_lanes, double length, int road_categ, int max_mass_on_axis, double road_price_per_meter)
        {
            double traf_lanes_v;
            if (traffic_lanes <= 0) traf_lanes_v = 1;
            else
            {
                traf_lanes_v = 1 / (double) traffic_lanes;
            }
                
            double del_t;

            if (road_categ <= 5 | max_mass_on_axis >= 10)
            {
                if (max_mass_on_axis >= 25 & road_categ <= 4) del_t = 0.1;
                else del_t = 0.05;
            }
            else del_t = 0;

            //Road_price_drop inst = new Road_price_drop();

            //inst.R = del_t * traf_lanes_v;
            //inst.damage_in_price = inst.R * road_price_per_meter * length;
            double R, dmg;
            R = del_t * traf_lanes_v;
            dmg = R * road_price_per_meter * length;


            dmg_sum += dmg;

            return dmg;
        }

        //public double evaluate_all_damage()
        //{
        //    double dmg_sum = 0;

        //    foreach(var item in drp_lst)
        //    {
        //        dmg_sum += item.damage_in_price;
        //    }

        //    return dmg_sum;
        //}

    }

    //class road_data_dmg : IEnumerable<road_data_dmg>
    //{
    //    public double R { get; set; }
    //    public double damage_in_price { get; set; }

    //    IEnumerator<road_data_dmg> IEnumerable<road_data_dmg>.GetEnumerator()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    IEnumerator IEnumerable.GetEnumerator()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
