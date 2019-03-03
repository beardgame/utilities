using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FsCheck.Xunit;

namespace Bearded.Utilities.Tests.Core.Random
{
    public class StaticRandomTests
    {
        public class TheSeedWithMethod : SharedTests.TheSeedMethod
        {
            protected override int CallMethod()
                => StaticRandom.Int();

            protected override void Seed(int seed)
                => StaticRandom.SeedWith(seed);
        }

        public class TheIntMethod : SharedTests.TheIntMethod
        {
            protected override int CallMethod()
                => StaticRandom.Int();

            protected override void Seed(int seed)
                => StaticRandom.SeedWith(seed);
        }
        
        public class TheIntWithMaxMethod : SharedTests.TheIntWithMaxMethod
        {
            protected override int CallMethod(int parameter) => StaticRandom.Int(parameter);

            protected override void Seed(int seed) => StaticRandom.SeedWith(seed);
        }

        public class TheIntWithMinAndMaxMethod : SharedTests.TheIntWithMinAndMaxMethod
        {
            protected override int CallMethod(int parameter1, int parameter2)
                => StaticRandom.Int(parameter1, parameter2);

            protected override void Seed(int seed)
                => StaticRandom.SeedWith(seed);
        }

        public class TheFloatMethod : SharedTests.TheFloatMethod
        {
            protected override float CallMethod()
                => StaticRandom.Float();

            protected override void Seed(int seed)
                => StaticRandom.SeedWith(seed);
        }

        public class TheFloatWithOneBoundMethod : SharedTests.TheFloatWithOneBoundMethod
        {
            protected override float CallMethod(float parameter)
                => StaticRandom.Float(parameter);

            protected override void Seed(int seed)
                => StaticRandom.SeedWith(seed);
        }

        public class TheFloatWithTwoBoundsMethod : SharedTests.TheFloatWithTwoBoundsMethod
        {
            protected override float CallMethod(float parameter1, float parameter2)
                => StaticRandom.Float(parameter1, parameter2);

            protected override void Seed(int seed)
                => StaticRandom.SeedWith(seed);
        }

        public class TheDoubleMethod : SharedTests.TheDoubleMethod
        {
            protected override double CallMethod()
                => StaticRandom.Double();

            protected override void Seed(int seed)
                => StaticRandom.SeedWith(seed);
        }

        public class TheDoubleWithOneBoundMethod : SharedTests.TheDoubleWithOneBoundMethod
        {
            protected override double CallMethod(double parameter)
                => StaticRandom.Double(parameter);

            protected override void Seed(int seed)
                => StaticRandom.SeedWith(seed);
        }

        public class TheDoubleWithTwoBoundsMethod : SharedTests.TheDoubleWithTwoBoundsMethod
        {
            protected override double CallMethod(double parameter1, double parameter2)
                => StaticRandom.Double(parameter1, parameter2);

            protected override void Seed(int seed)
                => StaticRandom.SeedWith(seed);
        }

        public class TheSignMethod : SharedTests.TheSignMethod
        {
            protected override int CallMethod()
                => StaticRandom.Sign();

            protected override void Seed(int seed)
                => StaticRandom.SeedWith(seed);
        }

        public class TheBoolMethod : SharedTests.TheBoolMethod
        {
            protected override bool CallMethod()
                => StaticRandom.Bool();

            protected override void Seed(int seed)
                => StaticRandom.SeedWith(seed);
        }
        
        public class TheBoolWithParameterMethod : SharedTests.TheBoolWithParameterMethod
        {
            protected override bool CallMethod(double parameter)
                => StaticRandom.Bool(parameter);

            protected override void Seed(int seed)
                => StaticRandom.SeedWith(seed);
        }

        public class TheDiscretiseMethod : SharedTests.TheDiscretiseMethod
        {
            protected override int CallMethod(float parameter)
                => StaticRandom.Discretise(parameter);

            protected override void Seed(int seed)
                => StaticRandom.SeedWith(seed);
        }
    }
}
