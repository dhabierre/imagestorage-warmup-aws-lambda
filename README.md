# imagestorage-warmup-aws-lambda

This lambda is used to warmup images that are uploaded from the ImageStorage Web Api. When a new image is uploaded the ImageStorage Web Api sends a message in a SQS. This lambda is triggered on SQS event, retrieves the urls and warms them.

## Tools

- [AWS Toolkit for Visual Studio 2017](https://marketplace.visualstudio.com/items?itemName=AmazonWebServices.AWSToolkitforVisualStudio2017)

- [Amazon LAmbda Tools](https://github.com/aws/aws-extensions-for-dotnet-cli#aws-lambda-amazonlambdatools)

  ```
  dotnet tool install -g Amazon.Lambda.Tools
  ```

## Test the lambda

You can use the unit tests with fake SQS event.

Or press F5, you should see the AWS .NET Mock Lambda Test Tool WebUI.\
(**Test Invoke ** > **Example Requests** > **AWS** > **SQS** > in the **body** field enter an url)