using static Bearded.Utilities.StaticRandom;

namespace Bearded.Utilities.Tests.Random;

public class StaticRandomTests
{
    public class TheSeedWithMethod : SharedTests.TheSeedMethod
    {
        protected override void Setup()
            => Setup(SeedWith, Int);
    }

    public class TheIntMethod : SharedTests.TheIntMethod
    {
        protected override void Setup()
            => Setup(SeedWith, Int);
    }

    public class TheIntWithMaxMethod : SharedTests.TheIntWithMaxMethod
    {
        protected override void Setup()
            => Setup(SeedWith, Int);
    }

    public class TheIntWithMinAndMaxMethod : SharedTests.TheIntWithMinAndMaxMethod
    {
        protected override void Setup()
            => Setup(SeedWith, Int);
    }

    public class TheLongMethod : SharedTests.TheLongMethod
    {
        protected override void Setup()
            => Setup(SeedWith, Long);
    }

    public class TheLongWithMaxMethod : SharedTests.TheLongWithMaxMethod
    {
        protected override void Setup()
            => Setup(SeedWith, Long);
    }

    public class TheLongWithMinAndMaxMethod : SharedTests.TheLongWithMinAndMaxMethod
    {
        protected override void Setup()
            => Setup(SeedWith, Long);
    }

    public class TheFloatMethod : SharedTests.TheFloatMethod
    {
        protected override void Setup()
            => Setup(SeedWith, Float);
    }

    public class TheFloatWithOneBoundMethod : SharedTests.TheFloatWithOneBoundMethod
    {
        protected override void Setup()
            => Setup(SeedWith, Float);
    }

    public class TheFloatWithTwoBoundsMethod : SharedTests.TheFloatWithTwoBoundsMethod
    {
        protected override void Setup()
            => Setup(SeedWith, Float);
    }

    public class TheDoubleMethod : SharedTests.TheDoubleMethod
    {
        protected override void Setup()
            => Setup(SeedWith, Double);
    }

    public class TheDoubleWithOneBoundMethod : SharedTests.TheDoubleWithOneBoundMethod
    {
        protected override void Setup()
            => Setup(SeedWith, Double);
    }

    public class TheDoubleWithTwoBoundsMethod : SharedTests.TheDoubleWithTwoBoundsMethod
    {
        protected override void Setup()
            => Setup(SeedWith, Double);
    }

    public class TheSignMethod : SharedTests.TheSignMethod
    {
        protected override void Setup()
            => Setup(SeedWith, Sign);
    }

    public class TheBoolMethod : SharedTests.TheBoolMethod
    {
        protected override void Setup()
            => Setup(SeedWith, Bool);
    }

    public class TheBoolWithParameterMethod : SharedTests.TheBoolWithParameterMethod
    {
        protected override void Setup()
            => Setup(SeedWith, Bool);
    }

    public class TheDiscretiseMethod : SharedTests.TheDiscretiseMethod
    {
        protected override void Setup()
            => Setup(SeedWith, Discretise);
    }

    public class TheNormalFloatMethod : SharedTests.TheNormalFloatMethod
    {
        protected override void Setup()
            => Setup(SeedWith, NormalFloat);
    }

    public class TheNormalFloatWithParametersMethod : SharedTests.TheNormalFloatWithParametersMethod
    {
        protected override void Setup()
            => Setup(SeedWith, NormalFloat);
    }

    public class TheNormalDoubleMethod : SharedTests.TheNormalDoubleMethod
    {
        protected override void Setup()
            => Setup(SeedWith, NormalDouble);
    }

    public class TheNormalDoubleWithParametersMethod : SharedTests.TheNormalDoubleWithParametersMethod
    {
        protected override void Setup()
            => Setup(SeedWith, NormalDouble);
    }
}
