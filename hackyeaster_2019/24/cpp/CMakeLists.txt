cmake_minimum_required(VERSION 3.13)
project(capteg)

set(CMAKE_CXX_STANDARD 14)
find_package(OpenCV REQUIRED)

add_executable(capteg main.cpp)
target_link_libraries(capteg ${OpenCV_LIBS})
