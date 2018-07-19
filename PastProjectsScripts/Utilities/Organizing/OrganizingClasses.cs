using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NS_Utilities.NS_Organizing
{
    public class Pair<A,B>
    {
        public A m_A;
        public B m_B;

        public Pair(A a, B b)
        {
            m_A = a;
            m_B = b;
        }
    }
}