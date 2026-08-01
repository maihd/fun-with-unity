using System;
using Unity.Burst;
using Unity.Collections;


[BurstCompile]
public struct Handle
{
    public int index;
    public int generation;
}


[BurstCompile]
public struct SparseIndex
{
    public int indexOrNext;
    public int generation;
}


[BurstCompile]
public unsafe struct HandleMap<T>
    where T: unmanaged
{
    public NativeList<T>                elements;
    public NativeList<Handle>           handles;
    public NativeList<SparseIndex>      sparseIndices;
    public int                          nextIndex;


    public int Count => elements.Length;


    public T* this[Handle handle]
    {
        get
        {
            if (handle.index >= sparseIndices.Length)
            {
                return null;
            }

            var entry = sparseIndices[handle.index];
            if (entry.generation == handle.generation)
            {
                fixed (T* ptr = &elements.ElementAt(entry.indexOrNext))
                {
                    return ptr;
                }
            }

            return null;
        }
    }

    public Handle Add(T value)
    {
        Handle handle;

        var entryIndex = nextIndex;
        if (entryIndex < sparseIndices.Length)
        {
            ref var entry = ref sparseIndices.ElementAt(entryIndex);
            entry.generation += 1;

            var index = handles.Length;
            handle = new Handle
            {
                index = index,
                generation = entry.generation
            };

            nextIndex = entry.indexOrNext;
            entry.indexOrNext = index;
        }
        else
        {
            entryIndex = sparseIndices.Length;
            var index = handles.Length;
            handle = new Handle
            {
                index = index,
                generation = 0,
            };

            var entry = new SparseIndex
            {
                indexOrNext = index,
                generation = 0,  
            };

            sparseIndices.Add(entry);
            nextIndex = entryIndex;
        }

        handles.Add(handle);
        elements.Add(value);

        return handle;
    }

    public bool Remove(Handle handle)
    {
        if (handle.index >= handles.Length)
        {
            return false;
        }

        var entryIndex = handle.index;
        ref var entry = ref sparseIndices.ElementAt(entryIndex);
        if (handle.generation != entry.generation)
        {
            return false;
        }

        entry.generation += 1;

        var index = entry.indexOrNext;
        nextIndex = entryIndex;

        handles.RemoveAtSwapBack(index);
        elements.RemoveAtSwapBack(index);

        if (index < handles.Length)
        {
            var swapEntryIndex = handles[index].index;
            ref var swapEntry = ref sparseIndices.ElementAt(swapEntryIndex);
            swapEntry.indexOrNext = index;
        }

        return true;
    }
}