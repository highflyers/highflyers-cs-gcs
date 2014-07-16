cmake_minimum_required (VERSION 2.6)
project (digits)
find_package(OpenCV REQUIRED)
set(CMAKE_CXX_FLAGS "-Wall -Wextra")
add_executable(digits digits.cpp)
target_link_libraries(digits ${OpenCV_LIBS})

