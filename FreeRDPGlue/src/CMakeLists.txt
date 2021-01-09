include(GenerateExportHeader)

add_executable(FreeRDPGlueDriver "main.c")
target_link_libraries(FreeRDPGlueDriver PRIVATE FreeRDPGlue)

add_custom_command (TARGET FreeRDPGlueDriver POST_BUILD
    COMMAND ${CMAKE_COMMAND} -E copy_if_different $<TARGET_FILE:freerdp> $<TARGET_FILE:winpr> $<TARGET_FILE:freerdp-client> $<TARGET_FILE_DIR:FreeRDPGlueDriver>
)

add_library(FreeRDPGlue SHARED "glue.h"
                               "glue.c"
                               "context_update.h"
                               "context_update.c"
                               "glue_update.h"
                               "rdpgfx.h"
                               "rdpgfx.c"
                               "cliprdr.h"
                               "cliprdr.c"
                               "${CMAKE_CURRENT_BINARY_DIR}/freerdpglue_export.h")
target_link_libraries(FreeRDPGlue PUBLIC freerdp freerdp-client ws2_32)
generate_export_header(FreeRDPGlue)
target_include_directories(FreeRDPGlue PUBLIC ${CMAKE_CURRENT_BINARY_DIR})

add_custom_command (TARGET FreeRDPGlue POST_BUILD
    COMMAND ${CMAKE_COMMAND} -E copy_if_different $<TARGET_FILE:freerdp> $<TARGET_FILE:winpr> $<TARGET_FILE:freerdp-client> $<TARGET_FILE_DIR:FreeRDPGlue>
)