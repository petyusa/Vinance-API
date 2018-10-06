﻿namespace Vinance.Contracts.Interfaces
{
    public interface IFactory<out T>
    {
        T Create();
    }
}