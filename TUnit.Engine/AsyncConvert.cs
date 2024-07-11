﻿namespace TUnit.Engine;

public static class AsyncConvert
{
    public static Task Convert(Action action)
    {
        action();
        return Task.CompletedTask;
    }

    public static async Task Convert(Func<Task> action)
    {
        await action();
    }

    public static async Task Convert(Func<ValueTask> action)
    {
        await action();
    }
}