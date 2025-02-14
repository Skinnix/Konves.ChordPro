﻿using Konves.ChordPro.DirectiveHandlers;
using Konves.ChordPro.Directives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Konves.ChordPro
{
	public static class ChordProSerializer
	{
		public static Document Deserialize(Stream stream)
		{
			return Deserialize(stream, null);
		}

		public static Document Deserialize(Stream stream, IEnumerable<DirectiveHandler> customHandlers)
		{
			using (StreamReader reader = new StreamReader(stream))
			{
				Parser parser = new Parser(reader);
				return new Document(parser.Parse());
			}
		}

		public static void Serialize(Document document, TextWriter writer, SerializerSettings settings = null)
		{
			var index = DirectiveHandlerUtility.GetHandlersDictionaryByType(settings?.CustomHandlers);

			foreach (ILine line in document.Lines)
			{
				if (line is Directive)
					writer.WriteLine(index[line.GetType()].GetString(line as Directive, settings?.ShortenDirectives == true)); // TODO: harden
				else if (line is SongLine)
					// writer.WriteLine(line.ToString()); // TODO: fix
					Write(writer, line as SongLine);
				else if (line is TabLine)
					writer.WriteLine((line as TabLine).Text);
				else
					throw new ArgumentException("unknown line type");
			}
		}

		internal static void Write(TextWriter writer, SongLine line)
		{
			bool addSpace = false;

			foreach (Block block in line.Blocks)
			{
				if (block is Word)
                {
                    if (addSpace)
                        writer.Write(' ');

                    Word word = block as Word;

					foreach (Syllable syllable in word.Syllables)
					{
						if (syllable.Chord != null)
							writer.Write($"[{syllable.Chord.Text}]");

						writer.Write(syllable.Text);
                    }

                    addSpace = true;
                }
				else if (block is Chord)
                {
                    if (addSpace)
                        writer.Write(' ');

                    Chord chord = block as Chord;
					writer.Write($"[{chord.Text}]");

                    addSpace = true;
                }
				else if (block is Whitespace)
				{
					Whitespace whitespace = block as Whitespace;
                    writer.Write(new string(' ', whitespace.Length));
					addSpace = false;
                }
			}

			writer.WriteLine();
		}
	}
}
