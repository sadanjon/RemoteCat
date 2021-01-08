set(FREERDP_INSTALL_DIR "" CACHE PATH "FreeRDP installation directory")

set(FREERDP_CONFIG_CMAKE_FILE "${FREERDP_INSTALL_DIR}/lib/cmake/FreeRDP2/FreeRDPConfig.cmake")
set(FREERDP_INCLUDE_DIR "${FREERDP_INSTALL_DIR}/include/freerdp2")

set(FREERDP_CLIENT_CONFIG_CMAKE_FILE "${FREERDP_INSTALL_DIR}/lib/cmake/FreeRDP-Client2/FreeRDP-ClientConfig.cmake")

set(WINPR_CONFIG_CMAKE_FILE "${FREERDP_INSTALL_DIR}/lib/cmake/WinPR2/WinPRConfig.cmake")
set(WINPR_INCLUDE_DIR "${FREERDP_INSTALL_DIR}/include/winpr2")

if (NOT EXISTS "${FREERDP_CONFIG_CMAKE_FILE}")
    message(FATAL_ERROR "${FREERDP_CONFIG_CMAKE_FILE} not found")
endif()

if (NOT EXISTS "${FREERDP_CLIENT_CONFIG_CMAKE_FILE}")
    message(FATAL_ERROR "${FREERDP_CLIENT_CONFIG_CMAKE_FILE} not found")
endif()

if (NOT EXISTS "${WINPR_CONFIG_CMAKE_FILE}")
    message(FATAL_ERROR "${WINPR_CONFIG_CMAKE_FILE} not found")
endif()

include("${WINPR_CONFIG_CMAKE_FILE}")
include("${FREERDP_CONFIG_CMAKE_FILE}")
include("${FREERDP_CLIENT_CONFIG_CMAKE_FILE}")

target_include_directories(freerdp INTERFACE ${FREERDP_INCLUDE_DIR} ${WINPR_INCLUDE_DIR})