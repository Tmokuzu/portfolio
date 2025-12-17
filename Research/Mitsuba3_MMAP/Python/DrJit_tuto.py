import numpy as np
import drjit as dr

###Similarity with NumPy###

from drjit.llvm import Float, UInt32

# Create some floating-point arrays
a = Float([1.0, 2.0, 3.0, 4.0])
b = Float([4.0, 3.0, 2.0, 1.0])

# Perform simple arithmetic
c = a + 2.0 * b

print(f'c -> ({type(c)}) = {c}')

# Convert to NumPy array
d = np.array(c)

###Array construction routines###

print(f'd -> ({type(d)}) = {d}')

# Initialize floating-point array of size 5 with zeros
a = dr.zeros(Float, 5) # np.zeros(5)
print(f'dr.zeros: {a}')

# Initialize floating-point array of size 5 with a constant value
a = dr.full(Float, 0.1, 5) # np.ones(5, 0.4)
print(f'dr.full: {a}')

a = dr.arange(UInt32, 5) # np.arange(5)
print(f'dr.arange: {a}')

# Return evenly spaced numbers over a specified interval
a = dr.linspace(Float, 0.0, 2.0, 5) # np.linspace(0.0, 2.0, 5)
print(f'dr.linespace: {a}')

###Masking###
x = dr.arange(Float, 5)
m = x > 2.0 # True for all values of a that are greater than 2.0
y = dr.select(m, 4.0, 1.0) # Set the values greater than 2.0 to 4.0 otherwise to 1.0
print(f'x -> ({type(x)}) {x}')
print(f'm -> ({type(m)}) {m}')
print(f'y -> ({type(y)}) {y}')

###Basic math arithmetic###
s, c = dr.sincos(a)
m = dr.minimum(s, c)
print(f'm: {m}')

###Horizontal operations###
a = dr.arange(Float, 5) + 1
print(f'a: {a}')

# Horizontal sum
b = dr.sum(a) # np.sum(a)
print(f'dr.sum(a): {b}')

# Horizontal product
b = dr.prod(a) # np.prod(a)
print(f'dr.prod(a): {b}')

# Mean value over the entire array
b = dr.mean(a) # np.mean(a)
print(f'dr.mean(a): {b}')

m = a > 2
print(f'm: {m}')

# True if all value of the mask array are True
b = dr.all(m) # np.all(m)
print(f'dr.all(m): {b}')

# True if any value of the mask array are True
b = dr.any(m) # np.any(m)
print(f'dr.any(m): {b}')

# True if no value of the mask array are True
b = dr.none(m) # ~np.any(m)
print(f'dr.none(m): {b}')

###use the dr.gather routine to read entries from an Dr.Jit array###
source = dr.linspace(Float, 0, 1, 5)
indices = UInt32([1, 2]) # Only read the 2nd and 3rd elements of the source array
result = dr.gather(Float, source, indices)
print(f'source: {source}')
print(f'indices: {indices}')
print(f'result: {result}')

###can write entries at specific indices into a Dr.Jit array###
target = dr.zeros(Float, 5)
indices = UInt32([0, 3, 4]) # Write to the first and last two elements of the target array
source = Float([1.0, 2.0, 3.0])
dr.scatter(target, source, indices)
print(f'indices: {indices}')
print(f'source: {source}')
print(f'target: {target}')




