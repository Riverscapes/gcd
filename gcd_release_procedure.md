# GCD Release Procedure

## Phase 1 - Preparation

1. Git fetch

2. Check out `master` branch and confirm correct commit.

3. Update the [release notes ](http://gcd.riverscapes.xyz/release_notes.html) document

4. Set the release version for `GCDCore` project.

## Phase 2 - GCD AddIn

1. Ensure `GCDAddIn` project is loaded in solution.

2. Set the release version for `GCDAddIn` project.

3. Set the release version and date in the `Config.esriaddinx` file at the root of the `GCDAddIn` project.​

4. Ensure build configuration is `Release x86`.

5. Clean the solution.

6. Rebuild the solution.

7. Rename the Addin file from `GCDAddIn.esriAddIn` to `YYYY_MM_DD_GCDAddIn_7_0_00.esriAddIn`.


## Phase 3 - GCD Standalone

1. *Unload* the `GCDAddIn` project in Visual Studio.
2. Clean Solution.
3. Rebuild solution.
4. Review binary output folder and ensure all the necessary dependencies are present.
5. ​