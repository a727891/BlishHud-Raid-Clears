import ctypes
import mmap
import time
import sys

class Link(ctypes.Structure):
    _fields_ = [
        ("uiVersion", ctypes.c_uint32),           # 4 bytes
        ("uiTick", ctypes.c_ulong),               # 4 bytes
        ("fAvatarPosition", ctypes.c_float * 3),  # 3*4 bytes
        ("fAvatarFront", ctypes.c_float * 3),     # 3*4 bytes
        ("fAvatarTop", ctypes.c_float * 3),       # 3*4 bytes
        ("name", ctypes.c_wchar * 256),           # 512 bytes
        ("fCameraPosition", ctypes.c_float * 3),  # 3*4 bytes
        ("fCameraFront", ctypes.c_float * 3),     # 3*4 bytes
        ("fCameraTop", ctypes.c_float * 3),       # 3*4 bytes
        ("identity", ctypes.c_wchar * 256),       # 512 bytes
        ("context_len", ctypes.c_uint32),         # 4 bytes
        # ("context", ctypes.c_ubyte * 256),      # 256 bytes, see below
        # ("description", ctypes.c_wchar * 2048), # 4096 bytes, always empty
    ]

class Padding(ctypes.Structure):
    _fields_ = [
        ("description", ctypes.c_wchar * 2048) # 4096 bytes, empty
    ]

class Context(ctypes.Structure):
    _fields_ = [
        ("serverAddress", ctypes.c_ubyte * 28),   # 28 bytes
        ("mapId", ctypes.c_uint32),               # 4 bytes
        ("mapType", ctypes.c_uint32),             # 4 bytes
        ("shardId", ctypes.c_uint32),             # 4 bytes
        ("instance", ctypes.c_uint32),            # 4 bytes
        ("buildId", ctypes.c_uint32),             # 4 bytes
        ("uiState", ctypes.c_uint32),             # 4 bytes
        ("compassWidth", ctypes.c_uint16),        # 2 bytes
        ("compassHeight", ctypes.c_uint16),       # 2 bytes
        ("compassRotation", ctypes.c_float),      # 4 bytes
        ("playerX", ctypes.c_float),              # 4 bytes
        ("playerY", ctypes.c_float),              # 4 bytes
        ("mapCenterX", ctypes.c_float),           # 4 bytes
        ("mapCenterY", ctypes.c_float),           # 4 bytes
        ("mapScale", ctypes.c_float),             # 4 bytes
        ("processId", ctypes.c_uint32),           # 4 bytes
        ("mountIndex", ctypes.c_uint8),           # 1 byte
    ]

identityString ='{"name": "Deeos","profession": 4,"spec": 55,"race": 4,"map_id": 50,"world_id": 268435505,"team_color_id": 0,"commander": false,"fov": 0.873,"uisz": 1}'

class MumbleLink:
    
    def __init__(self):
        self.size_link = ctypes.sizeof(Link)
        self.size_context = ctypes.sizeof(Context)
        size_discarded = 256 - self.size_context + 4096 # empty areas of context and description
        
        # GW2 won't start sending data if memfile isn't big enough so we have to add discarded bits too
        memfile_length = self.size_link + self.size_context + size_discarded
        
        self.memfile = mmap.mmap(fileno=-1, length=memfile_length, tagname="MumbleLink", access=mmap.ACCESS_WRITE)
    
    def write(self, uiTick, uiState, ppid):
        data=Link(1,uiTick,(0,0,0),(0,0,0),(0,0,0),"Guild Wars 2",(0,0,0),(0,0,0),(0,0,0),identityString,48)
        ctx=Context((0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),0,0,0,0,0,uiState,0,0,0,0,0,0,0,0,ppid,9)
        padd = Padding()

        databuf = ctypes.string_at(ctypes.byref(data), ctypes.sizeof(data))
        ctxbuf = ctypes.string_at(ctypes.byref(ctx), ctypes.sizeof(ctx))
        padBuff = ctypes.string_at(ctypes.byref(padd), ctypes.sizeof(padd))
        #return buf

        # print(buf)
        self.memfile.seek(0)
        self.memfile.write(databuf)
        self.memfile.write(ctxbuf)
        self.memfile.write(padBuff)

    
def main(ppid):
    ml = MumbleLink()
    uiTick = 1
    uiState= 8 # 8=Game has focus
    ml.write(uiTick, uiState, ppid)
    while True:
        time.sleep(0.03)
        uiTick+=1
        ml.write(uiTick, uiState, ppid)


if __name__ == "__main__":
    if(len(sys.argv)==2):
        print(sys.argv[1])
        main(int(sys.argv[1]))
    else:
        main(0)