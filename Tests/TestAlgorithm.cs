using System;
using Domain;
using Domain.LearnMethods;
using NUnit.Framework;

namespace Tests
{
    public class TestAlgorithm
    {
        [SetUp]
        public void Setup()
        {
        }

        private void TestLearnMethod(ILearnMethod method, TimeSpan expected)
        {
            var parameters = method.GetParameters();
            var user = new User("1");
            var card = new Card(user, Guid.Empty, "test", "test", parameters);
            parameters.LearnCard(card, 2);
            Assert.True(card.TimeBeforeLearn == expected);
        }

        [Test]
        public void TestLineLearnMethod()
        {
            var excepted = new TimeSpan(1, 0, 0, 0, 0) * 2;
            TestLearnMethod(new LineLearnMethod(), excepted);
        }

        [Test]
        public void TestSuperMemo2LearnMethod()
        {
            var excepted = new TimeSpan(1, 0, 0, 0, 0);
            TestLearnMethod(new SuperMemo2LearnMethod(), excepted);
        }
    }
}