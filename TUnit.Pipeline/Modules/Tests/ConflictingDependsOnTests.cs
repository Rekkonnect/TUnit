﻿using FluentAssertions;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Parsers.NUnitTrx;
using ModularPipelines.Extensions;
using ModularPipelines.Git.Extensions;
using File = ModularPipelines.FileSystem.File;

namespace TUnit.Pipeline.Modules.Tests;

public class ConflictingDependsOnTests : TestModule
{
    protected override async Task<TestResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var file = Guid.NewGuid().ToString("N") + ".trx";
        
        await RunTestsWithFilter(context, 
            "/*/*/ConflictingDependsOnTests/*",
            [
                result => result.Successful.Should().BeFalse(),
                result => result.Total.Should().Be(2),
                result => result.Passed.Should().Be(0),
                result => result.Failed.Should().Be(2),
                result => result.Skipped.Should().Be(0),
            ],
            new RunOptions
            {
                AdditionalArguments = ["--report-trx", "--report-trx-filename", file],
            },  cancellationToken);

        var trxReport = new TrxParser().ParseTrxContents(await context.Git().RootDirectory.AssertExists().FindFile(x => x.Name == file).AssertExists().ReadAsync(cancellationToken));

        trxReport.UnitTestResults.First(x => x.TestName == "Test1").Output?.ErrorInfo?.Message.Should().Contain("DependencyConflictException");
        trxReport.UnitTestResults.First(x => x.TestName == "Test2").Output?.ErrorInfo?.Message.Should().Contain("DependencyConflictException");
        
        return null;
    }
}