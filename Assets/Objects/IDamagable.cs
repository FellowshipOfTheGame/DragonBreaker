using System;

interface IDamagable
{
    void hit(float energy, Action<int> callback = null);
}