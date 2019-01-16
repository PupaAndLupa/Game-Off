from PIL import Image
import sys

def main():
    infile = sys.argv[1]

    image = Image.open(infile)
    inPixels = image.load()

    width = image.size[0]
    height = image.size[1]

    for i in range(width):
        for j in range(height):
            if inPixels[i, j] == (255, 255, 255, 255):
                inPixels[i, j] = (255, 255, 255, 0)

    image.save(infile)

if __name__ == "__main__":
    main()
