#pragma once

namespace AnarchyHook {

typedef void Message_t;
typedef Message_t* (*DataBlockToMessage)(int size, void* pDataBlock);

class EmptyClass {};
typedef int (EmptyClass::*Connection_t_Send)(unsigned int pArg1, unsigned int pArg2, void const* pArg3);

}
