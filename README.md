# sl-Hive

This package makes use of code from https://gitlab.syncad.com/hive/hive-net for broadcasting transactions to hive.

The following hive engine requests are currently supported

* condenser_api -> get_accounts
* condenser_api -> get_block_header
* condenser_api -> get_block
* database_api -> get_dynamic_global_properties
* condenser_api -> broadcast_transaction