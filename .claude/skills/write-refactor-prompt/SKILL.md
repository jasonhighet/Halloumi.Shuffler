---
name: write-refactor-prompt
description: Generate a detailed, self-contained refactoring prompt for another AI that has no codebase context. Reads relevant files, builds the prompt. Use when the user says "write me a detailed prompt for another AI to fix this" or similar.
disable-model-invocation: true
argument-hint: task/violation description
---

The user wants a detailed, self-contained prompt for another AI instance that has **zero prior context** about this codebase. That AI should only produce a **plan** — not write any code.

The task to address is:

$ARGUMENTS

---

## Your workflow

### Step 1 — Gather context

Always read:
- `CLAUDE.md` — architecture rules, project structure, intentional exceptions

Then parse the task description above and read every specific file mentioned in it. Use grep to find the actual lines of interest so you can quote them accurately.

Also find one or two existing examples in the codebase that already follow the correct pattern for whatever is being fixed — the other AI should model its plan on those.

### Step 2 — Compose the prompt

Write a single, self-contained prompt. It must contain everything the other AI needs — assume it cannot read any files.

**Section A: Project context**

Summarize the project environment:
- Primary language and frameworks (e.g. C#, .NET, React, etc.)
- Build and test commands
- Architecture overview (key layers and dependencies)
- Core design principles or "golden rules" from `CLAUDE.md` that apply to this task

**Section B: Relevant APIs and Patterns**

Paste the relevant public signatures (methods, interfaces, components) that are part of the solution. Also show the "correct" usage examples you found in Step 1.

**Section C: The task — file by file**

For each file involved:
- Quote the current code that needs to change
- Describe what needs to happen: new methods, moved logic, changed call sites, etc.
- Note edge cases or design decisions that aren't obvious from the code

**Section D: Output instruction** (include this verbatim)

> **Only produce a detailed implementation plan — do not write any code.** List every change required, file by file, method by method. For each new method, specify name, parameters, return type, and what it delegates to internally. Identify any ambiguities or design decisions that need to be resolved before coding begins.

### Step 3 — Deliver

Display the full generated prompt in this conversation inside a triple-backtick block.

