#include "glue.h"

int main(int argc, char *argv[])
{
	FrgMainOptions options = { 0 };
	options.argc = argc;
	options.argv = argv;

	return frgMain(&options);
}
