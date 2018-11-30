namespace ImageStorage.Lambda.Warmup.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Amazon.Lambda.SQSEvents;
    using Amazon.Lambda.TestUtilities;
    using Xunit;

    public class FunctionTest
    {
        [Fact]
        public async Task InvokeLambda_BadUrl()
        {
            var url = "BadUrl";

            var sqsEvent = new SQSEvent
            {
                Records = new List<SQSEvent.SQSMessage> { new SQSEvent.SQSMessage { Body = url } }
            };

            var logger = new TestLambdaLogger();
            var context = new TestLambdaContext { Logger = logger };

            using (var function = new Function())
            {
                await function.FunctionHandler(sqsEvent, context);

                Assert.Contains("exiting", logger.Buffer.ToString());
            }
        }


        [Fact]
        public async Task InvokeLambda_ImageNotFound()
        {
            var url = "https://www.google.fr/images/branding/googlelogo/1x/image_no_existing.png";

            var sqsEvent = new SQSEvent
            {
                Records = new List<SQSEvent.SQSMessage> { new SQSEvent.SQSMessage { Body = url } }
            };

            var logger = new TestLambdaLogger();
            var context = new TestLambdaContext { Logger = logger };

            using (var function = new Function())
            {

                await function.FunctionHandler(sqsEvent, context);

                Assert.Contains("Exception", logger.Buffer.ToString());
                Assert.Contains("404 (Not Found)", logger.Buffer.ToString());
            }
        }

        [Fact]
        public async Task InvokeLambda_OK()
        {
            var url = "https://www.google.fr/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png";

            var sqsEvent = new SQSEvent
            {
                Records = new List<SQSEvent.SQSMessage> { new SQSEvent.SQSMessage { Body = url } }
            };

            var logger = new TestLambdaLogger();
            var context = new TestLambdaContext { Logger = logger };

            var function = new Function();

            await function.FunctionHandler(sqsEvent, context);

            Assert.DoesNotContain(logger.Buffer.ToString(), "exiting");
            Assert.DoesNotContain(logger.Buffer.ToString(), "Exception");
        }
    }
}
