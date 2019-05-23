# Day 18: Be Evil

For this challenge we get a jar file. When decompiled with CFR we get several classes. All of them consist of a single
static byte array except from Evilist and EvilLoader:

```java
public class Evilist {
    public static void main(String[] args) throws Exception {
        EvilLoader evilLoader = new EvilLoader(Evilist.class.getClassLoader());

        EvilLoader loader = new EvilLoader(Evilist.class.getClassLoader());
        Class<?> clazz = loader.loadClass("hackvent2018.evil.EvilWindow");
        clazz.newInstance();
    }
}

public class EvilLoader extends ClassLoader {

    EvilLoader(ClassLoader parent) {
	super(parent);
    }

    private Class getClass(String name) throws ClassNotFoundException {
        byte[] b = loadEvilClass(name);
        return defineClass(name, b, 0, b.length);
    }

    public Class<?> loadClass(String name) throws ClassNotFoundException {
        try {
            return getClass(name);
        } catch (ClassFormatError cfe) {
            return super.loadClass(name);
        } catch (ClassNotFoundException cnfe) {}

        return super.loadClass(name);
    }

    private byte[] loadEvilClass(String name) throws ClassNotFoundException {
        Class clazz = EvilLoader.class.getClassLoader().loadClass(name);
        try {
            return (byte[])clazz.getField("b").get(clazz);

        } catch (IllegalArgumentException|IllegalAccessException|NoSuchFieldException|SecurityException|ClassFormatError e1) {
            throw new ClassNotFoundException(e1.toString());
        }
    }
}
```

As soon as a class gets loaded the `loadEvilClass`gets called. This method loads the real class first and then returns
the byte array `b` of the class. So what we have here are bytes of the java class format. We can now use those to get
the effective classes that are loaded at runtime.

The decoded EvilEvent class contains this Java code:

```java
public class EvilEvent {
    private static byte[] b = new byte[]{-83, 8, 119, 19, 73, 17, 2, 83, 126, 17, 33, 119, 115, 6, 38, 16, 26, 23, 10, 127, 20, 85, 81, 47, 13, 88, 43, 0, 70, 27, -122, 8, 83, 17, 125, 46, 78, 64, 89, 78, 41};

    static String eventResult() {
        byte[] x = xor(b, NotEvil.b, 0);
        x = xor(x, Evil.b, 100);
        x = xor(x, Sad.b, 200);
        x = xor(x, Question.b, 300);
        return new String(x);
    }

    private static byte[] xor(byte[] c, byte[] b, int offset) {
        byte[] x = new byte[c.length];

        for(int i = 0; i < c.length; ++i) {
            x[i] = (byte)(c[i] ^ b[i + offset]);
        }

        return x;
    }
}
```

All we have to do now is to call `eventResult` to get the flag.

