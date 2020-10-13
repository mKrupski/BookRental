using System;
using System.Collections.Generic;
using System.Text;

namespace BookRental.Infrastructure.Initializer
{
    public interface IDbInitializer
    {
        void Initialize();
    }
}
