using System;
using System.Collections.Generic;

namespace RockPaperScissors.Tasks.Infrastructure
{
    public delegate IEnumerable<object> MultiInstanceFactory(Type serviceType);
}