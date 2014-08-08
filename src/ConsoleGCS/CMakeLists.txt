cmake_minimum_required(VERSION 2.8.9)

project(ConsoleGCS)

SET(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -Wall -pedantic -std=c++11")
SET(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -Wl,--no-as-needed")

INCLUDE(FindPkgConfig)

find_package( OpenCV REQUIRED )
include_directories( ${OpenCV_INCLUDE_DIRS} )

pkg_check_modules(GSTREAMER REQUIRED gstreamer-1.0)
pkg_check_modules(GSTREAMER_APP REQUIRED gstreamer-app-1.0)
include_directories(${GSTREAMER_INCLUDE_DIRS})

add_executable(console-gcs 
  main.cpp
  Image_processor.cpp
  Image_provider.cpp new_digits.cpp
)

target_link_libraries(console-gcs 
  ${GSTREAMER_LIBRARIES}
  ${GSTREAMER_APP_LIBRARIES}
  ${OpenCV_LIBS}
)