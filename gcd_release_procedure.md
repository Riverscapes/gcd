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
3. Review `Deploy` tab in Standalone project and ensure that `Sign Manifests` is turned on and that the `GCDStandalone `certificate is active.
4. Rebuild solution.
5. Review binary output folder and ensure all the necessary dependencies are present.
6. Publish to local `Deploy` folder.
7. Review deploy folder and ensure all necessary dependencies are present.
8. Run batch file to deploy to AWS.

## Phase 4 - Git

1. Close Visual Studio (forces some changes to project files to get saved).
2. Review changes to `GCDStandalone` project and **discard the hunk** that turns on the manifest signing.
3. Commit changes with the message `release preparation`
4. Push.
5. Tag with release number `7.x.xx_BETA`

## Phase 5 - GitHub

1. Draft a new release using the tag created in the previous phase.
2. Make a copy of the GCD `setup.exe` file from the `deploy` folder and call it `YYYY_MM_DD_GCDStandalone_7_0_03.exe`.
3. Attach **both** the GCDAddin binary **and** the renamed setup EXE file to the release in GitHub.
4. Publish.