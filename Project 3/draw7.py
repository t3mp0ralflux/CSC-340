from PIL import Image, ImageDraw

im = Image.new('RGBA', (512, 512), (0,0,0,0))
draw = ImageDraw.Draw(im)
draw.rectangle(((180,220),(290,235)),fill='white')
draw.rectangle(((180,220), (260, 360)), fill='white')
draw.rectangle(((180,360),(290, 345)), fill='white')
im.show()

imTest = Image.new('RGBA', (512,512), (0,0,0,0))
drawTest = ImageDraw.Draw(imTest)
drawTest.rectangle(((0,0),(30,15)), fill='white')
drawTest.rectangle(((0,0),(15,120)), fill='white')
drawTest.rectangle(((0,120),(30,105)), fill='white')
imTest.show()
