using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


interface IBaseAI
{
    GameObject FindRandomTarget(int sightrange);
    void AttackTarget(GameObject target);
    void Movement();
}

