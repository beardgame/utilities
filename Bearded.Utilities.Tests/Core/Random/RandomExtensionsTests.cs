using System;

namespace Bearded.Utilities.Tests.Random;

public class RandomExtensionsTests
{
    private static void setup<T>(BaseTests.RandomMethodBase<T> tests, Func<System.Random, T> getMethod)
    {
        tests.Seed = seed => tests.CallMethod = getMethod(new System.Random(seed));
    }
        
    public class TheSeedWithMethod : SharedTests.TheSeedMethod
    {
        protected override void Setup()
            => setup(this, r => r.Next);
    }

    public class TheIntMethod : SharedTests.TheIntMethod
    {
        protected override void Setup()
            => setup(this, r => r.Next);
    }
        
    public class TheIntWithMaxMethod : SharedTests.TheIntWithMaxMethod
    {
        protected override void Setup()
            => setup(this, r => r.Next);
    }

    public class TheIntWithMinAndMaxMethod : SharedTests.TheIntWithMinAndMaxMethod
    {
        protected override void Setup()
            => setup(this, r => r.Next);
    }
        
    public class TheLongMethod : SharedTests.TheLongMethod
    {
        protected override void Setup()
            => setup(this, r => r.NextLong);
    }
        
    public class TheLongWithMaxMethod : SharedTests.TheLongWithMaxMethod
    {
        protected override void Setup()
            => setup(this, r => r.NextLong);
    }

    public class TheLongWithMinAndMaxMethod : SharedTests.TheLongWithMinAndMaxMethod
    {
        protected override void Setup()
            => setup(this, r => r.NextLong);
    }

    public class TheFloatMethod : SharedTests.TheFloatMethod
    {
        protected override void Setup()
            => setup(this, r => r.NextFloat);
    }

    public class TheFloatWithOneBoundMethod : SharedTests.TheFloatWithOneBoundMethod
    {
        protected override void Setup()
            => setup(this, r => r.NextFloat);
    }

    public class TheFloatWithTwoBoundsMethod : SharedTests.TheFloatWithTwoBoundsMethod
    {
        protected override void Setup()
            => setup(this, r => r.NextFloat);
    }

    public class TheDoubleMethod : SharedTests.TheDoubleMethod
    {
        protected override void Setup()
            => setup(this, r => r.NextDouble);
    }

    public class TheDoubleWithOneBoundMethod : SharedTests.TheDoubleWithOneBoundMethod
    {
        protected override void Setup()
            => setup(this, r => r.NextDouble);
    }

    public class TheDoubleWithTwoBoundsMethod : SharedTests.TheDoubleWithTwoBoundsMethod
    {
        protected override void Setup()
            => setup(this, r => r.NextDouble);
    }

    public class TheSignMethod : SharedTests.TheSignMethod
    {
        protected override void Setup()
            => setup(this, r => r.NextSign);
    }

    public class TheBoolMethod : SharedTests.TheBoolMethod
    {
        protected override void Setup()
            => setup(this, r => r.NextBool);
    }
        
    public class TheBoolWithParameterMethod : SharedTests.TheBoolWithParameterMethod
    {
        protected override void Setup()
            => setup(this, r => r.NextBool);
    }

    public class TheDiscretiseMethod : SharedTests.TheDiscretiseMethod
    {
        protected override void Setup()
            => setup(this, r => r.Discretise);
    }

    public class TheNormalFloatMethod : SharedTests.TheNormalFloatMethod
    {
        protected override void Setup()
            => setup(this, r => r.NormalFloat);
    }

    public class TheNormalFloatWithParametersMethod : SharedTests.TheNormalFloatWithParametersMethod
    {
        protected override void Setup()
            => setup(this, r => r.NormalFloat);
    }

    public class TheNormalDoubleMethod : SharedTests.TheNormalDoubleMethod
    {
        protected override void Setup()
            => setup(this, r => r.NormalDouble);
    }
        
    public class TheNormalDoubleWithParametersMethod : SharedTests.TheNormalDoubleWithParametersMethod
    {
        protected override void Setup()
            => setup(this, r => r.NormalDouble);
    }
}
